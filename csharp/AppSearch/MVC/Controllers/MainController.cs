using AppSearch.MVC.Helpers;
using AppSearch.MVC.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using AppSearch.MVC.Views;
using System.Windows.Media.Animation;
using AppSearch.CustomClasses.Tasks;
using System.Net;

namespace AppSearch.MVC.Controllers
{
    public class MainController(MainWindow view)
    {
        private readonly MainWindow _mainView = view;
        private DispatcherTimer _refreshTimer;
        private ConfigurationModel _config;
        private string _mainDirAppPath;
        private bool _envButtonClicked;
        private bool _clientsButtonClicked;
        private bool _mainTreeIsExpandedFlag;
        private Storyboard _spinAnimation;
        private PingBackgroundTask _pingBackgroundTask;
        private AppConfigEditor _editor;

        public ConfigurationModel Config
        {
            get { return _config; }
        }

        public void Initialize()
        {
            InitializeConfig();
            InitializeData();
            InitializeResources();
        }

        public void SearchBoxValueChanged()
        {
            _mainView.FilteredData.Refresh();
            FillTreeView();
        }

        public void FocusSearchBox()
        {
            _mainView.SearchTextBox.Focus();
        }

        public void RunApplication()
        {
            if (_mainView.DataGrid.SelectedItem is EnviromentModel row)
            {
                string? path = row.AppModel.TargetPath;
                if (path?.EndsWith(".scc") == true)
                {
                    var dirPath = new FileInfo(path).Directory?.FullName;
                    var appName = FileHelper.GetExecutiveFileFromSCCFile(path);
                    if (!string.IsNullOrEmpty(dirPath))
                        path = Path.Combine(dirPath, appName ?? row.AppModel.Name + ".exe");
                }
                RunApp(path);
            }
        }

        public void OpenUrl(OpenUrlKind kind)
        {
            if (_mainView.DataGrid.SelectedItem is EnviromentModel row)
            {
                string? url = null;
                switch(kind)
                {
                    case OpenUrlKind.SPECIFIC:
                        url = row.AppModel.WebServiceUrl;
                        break;
                    case OpenUrlKind.GCM:
                        url = row.WebServiceUrl;
                        break;
                    case OpenUrlKind.MOM:
                        if(!string.IsNullOrEmpty(row.WebServiceUrl))
                            url = row.WebServiceUrl.Remove(row.WebServiceUrl.IndexOf("gcm")) + "mom/console";
                        break;
                }
                OpenWebsite(url);
            }
        }

        public void RunApplicationFromTree(object sender)
        {
            if (sender is TreeView treeView && treeView.SelectedItem is SimpleNodeModel node && node.Childs?.Any() == false)
            {
                RunApp(node.TargetPath);
            }
        }

        public void ShowConfig()
        {
            string filePath = Path.Combine(_mainDirAppPath, Properties.Resources.ConfigFileName + ".xml");
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            MessageBox.Show(Properties.Resources.EditConfigFileWarning, Properties.Resources.Warning, MessageBoxButton.OK, MessageBoxImage.Hand);
        }

        public void Verify()
        {
            _spinAnimation.Begin();
            _ = _pingBackgroundTask.StartAsync();
        }

        public void Refresh()
        {
            RefreshGUI();
        }

        public void ShowEnviromentTree()
        {
            _envButtonClicked = true;
            _clientsButtonClicked = false;
            _mainView.EnvButtonText.FontWeight = FontWeights.Bold;
            _mainView.ClientsButtonText.FontWeight = FontWeights.Normal;
            FillTreeView();
        }

        public void ShowClientTree()
        {
            _envButtonClicked = false;
            _clientsButtonClicked = true;
            _mainView.EnvButtonText.FontWeight = FontWeights.Normal;
            _mainView.ClientsButtonText.FontWeight = FontWeights.Bold;
            FillTreeView();
        }

        public void ShowTree()
        {
            if (_mainView.TreeViewGrid.Visibility == Visibility.Visible)
            {
                _mainView.TreeViewGrid.Visibility = Visibility.Collapsed;
                _mainView.ShowHideTreeViewButton.Content = Properties.Resources.ShowTreeView;
                if(_mainView.MinHeight == _mainView.Height)
                    _mainView.Height -= _mainView.TreeViewGrid.Height;
            }
            else
            {
                _mainView.TreeViewGrid.Visibility = Visibility.Visible;
                _mainView.ShowHideTreeViewButton.Content = Properties.Resources.HideTreeView;
                if (_mainView.MinHeight == _mainView.Height)
                    _mainView.Height += _mainView.TreeViewGrid.Height;
            }
        }

        public void ExpandTree()
        {
            _mainTreeIsExpandedFlag = !_mainTreeIsExpandedFlag;
            ExpandAllNodes(_mainView.TreeData, _mainTreeIsExpandedFlag);

            _mainView.ExpandIcon.Kind = _mainTreeIsExpandedFlag ?
                MaterialDesignThemes.Wpf.PackIconKind.CollapseAll
                : MaterialDesignThemes.Wpf.PackIconKind.ExpandAll;

            _mainView.OnPropertyChanged(nameof(_mainView.TreeData));
        }

        public void SaveConfig()
        {
            try
            {
                ConfigHelper.SaveConfig(_config);
            }
            catch(Exception ex)
            {
                LogHelper.WriteLine(ex, _config, LogginLevel.ERROR);
            }
        }

        public void EditUrl()
        {
            if(_mainView.DataGrid.SelectedItem is EnviromentModel row && _editor?.Activate() != true)
            {
                _editor = new AppConfigEditor(this, row);
                _editor.Show();
            }
        }

        public void DeleteItem()
        {
            if (_mainView.DataGrid.SelectedItem is EnviromentModel row)
            {
                var result = MessageBox.Show(string.Format(Properties.Resources.DeleteQuestion, row.AppModel.Name), 
                    Properties.Resources.Question, MessageBoxButton.YesNoCancel, MessageBoxImage.Hand);

                if (result == MessageBoxResult.Yes)
                {
                    _mainView.Data.RemoveAt(_mainView.DataGrid.SelectedIndex);
                    FileHelper.RemoveFile(row.AppModel.TargetPath);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _mainView.FilteredData.Refresh();
                    });
                }
            }
        }

        public bool UpdateData(string envName, string url)
        {
            bool changed = false;
            foreach(var item in _mainView.DataGrid.ItemsSource)
            {
                if(item is EnviromentModel envModel && envModel.EnvName.Equals(envName))
                {
                    envModel.UpdateTargetUri(url);
                    envModel.AppModel.SetWebServiceUrl(url);
                    changed = true;
                }
            }
            if(changed)
            {
                bool savedInConfig = false;
                foreach(var item in _config.EnvList)
                {
                    if(item.EnvName.Equals(envName))
                    {
                        item.Url = url;
                        savedInConfig = true;
                    }
                }
                if(!savedInConfig)
                {
                    _config.EnvList.Add(new ConfigurationModel.EnviromentUrl
                    {
                        EnvName = envName,
                        Url = url
                    });
                }
            }
            return changed;
        }

        #region Private Methods

        private void InitializeResources()
        {
            _mainView.Title = Properties.Resources.ApplicationName;

            _mainView.SearchLabel.Content = Properties.Resources.SearchLabel;
            _mainView.ConfigButton.Content = Properties.Resources.ShowConfig;
            _mainView.ConfigButton.FontSize = 15;
            _mainView.VerifyButton.Content = Properties.Resources.Verify;
            _mainView.VerifyButton.FontSize = 20;
            _mainView.EnvButtonText.Text = Properties.Resources.Enviroment;
            _mainView.ClientsButtonText.Text = Properties.Resources.Clients;

            _mainView.ShowHideTreeViewButton.Content = Properties.Resources.ShowTreeView;
        }

        private void InitializeConfig()
        {
            _mainDirAppPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Properties.Resources.ApplicationName);

            if (!Directory.Exists(_mainDirAppPath))
            {
                Directory.CreateDirectory(_mainDirAppPath);
            }

            if (!Directory.Exists(Path.Combine(_mainDirAppPath, Properties.Resources.AppsDirName)))
            {
                Directory.CreateDirectory(Path.Combine(_mainDirAppPath, Properties.Resources.AppsDirName));
            }

            var config = ConfigHelper.LoadConfig(_mainDirAppPath);
            if (config != null)
            {
                _config = config;
            }
            else
            {
                LogHelper.WriteErrorLine(new Exception("Error durring cofing reading"));
            }
            LogHelper.Initialize(_mainDirAppPath);
        }

        private void InitializeData()
        {
            _mainTreeIsExpandedFlag = true;
            _mainView.Data = [];
            _mainView.TreeData = [];

#if DEBUG
            /*_mainView.Data.Add(new EnviromentModel("Q373", "PATHW", new AppModel("SoftMol", null), true, null));
            _mainView.Data.Add(new EnviromentModel("Q373", "PATHW", new AppModel("SoftDxp", null), true, null));
            _mainView.Data.Add(new EnviromentModel("Q373", "PATHW", new AppModel("SoftFlw", null), true, null));
            _mainView.Data.Add(new EnviromentModel("Q424", "PATHW", new AppModel("SoftMol", null), null, null));
            _mainView.Data.Add(new EnviromentModel("Q424", "PATHW", new AppModel("SoftDxp", null), null, null));
            _mainView.Data.Add(new EnviromentModel("Q368", "PATHW", new AppModel("SoftMol", null), false, null));
            _mainView.Data.Add(new EnviromentModel("Q368", "PATHW", new AppModel("SoftDxp", null), false, null));*/
            var item1 = new ConfigurationModel.ExchangeName("SoftBioChem", "SoftBio");
            var item2 = new ConfigurationModel.ExchangeName("SoftPathDx", "SoftDxp");
            var item3 = new ConfigurationModel.ExchangeName("SoftHLA", "SoftHla");
            if (!_config.UnwantedPartsAppNames.Any(item => item.From.Equals(item1.From)))
                _config.UnwantedPartsAppNames.Add(item1);
            if (!_config.UnwantedPartsAppNames.Any(item => item.From.Equals(item2.From)))
                _config.UnwantedPartsAppNames.Add(item2);
            if (!_config.UnwantedPartsAppNames.Any(item => item.From.Equals(item3.From)))
                _config.UnwantedPartsAppNames.Add(item3);
#endif

            _envButtonClicked = false;
            _mainView.EnvButtonText.FontWeight = FontWeights.Normal;
            _clientsButtonClicked = true;
            _mainView.ClientsButtonText.FontWeight = FontWeights.Bold;

            FillData();

            _mainView.FilteredData = CollectionViewSource.GetDefaultView(_mainView.Data);
            _mainView.FilteredData.Filter = FilterData;

            _spinAnimation = (Storyboard)_mainView.RefreshButton.Resources["SpinAnimation"];

            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(_config.RefreshTimerInterval)
            };
            _refreshTimer.Tick += TimerTick;
            _refreshTimer.Start();
            _pingBackgroundTask = new PingBackgroundTask(_mainView.Data, _config);
            _pingBackgroundTask.TaskCanceled += PingBackgroundTask_TaskCanceled;
            _pingBackgroundTask.TaskCompleted += PingBackgroundTask_TaskCompleted;

            _spinAnimation.Begin();
            _ = _pingBackgroundTask.StartAsync();
        }

        private void PingBackgroundTask_TaskCanceled(object sender, object arg)
        {
            if(_config.EnableLogging == LogginLevel.INFO)
            {
                MessageBox.Show(GetInfo(Properties.Resources.TaskEndedOnPing, Properties.Resources.TaskEnded,
                nameof(BackgroundTask.TaskCanceled), arg));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                _mainView.FilteredData.Refresh();
                int remainingTasks = _pingBackgroundTask.ActiveTasks;

                if (remainingTasks == 0)
                {
                    _spinAnimation.Stop();
                }
            });
        }

        private void PingBackgroundTask_TaskCompleted(object sender, object arg)
        {
            if(_config.EnableLogging == LogginLevel.INFO)
            {
                MessageBox.Show(GetInfo(Properties.Resources.TaskEndedOnPing, 
                    Properties.Resources.TaskEnded, nameof(BackgroundTask.TaskCompleted), arg), Properties.Resources.Info,
                    MessageBoxButton.OK, MessageBoxImage.Hand);
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                int remainingTasks = _pingBackgroundTask.ActiveTasks;

                if (remainingTasks == 0)
                {
                    UpdateDataVersion();
                    _spinAnimation.Stop();
                }
                _mainView.FilteredData.Refresh();
            });
        }

        private string GetInfo(string resources, string resources2, string methodName, object arg)
        {
            string info;
            if (arg is EnviromentModel env)
            {
                info = string.Format(resources, methodName,
                    env.EnvName, env.AppModel.Name, env.WebServiceUrl);
            }
            else
            {
                info = string.Format(resources2, methodName);
            }
            return info;
        }

        private bool FilterData(object item)
        {
            if (item is EnviromentModel model)
            {
                if (_mainView.SearchTextBox == null || string.IsNullOrWhiteSpace(_mainView.SearchTextBox.Text))
                    return true;

                return model.EnvName.Contains(_mainView.SearchTextBox.Text, StringComparison.OrdinalIgnoreCase) ||
                       model.ClientName.Contains(_mainView.SearchTextBox.Text, StringComparison.OrdinalIgnoreCase) ||
                       model.AppModel.Name.Contains(_mainView.SearchTextBox.Text, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private void FillData()
        {
            var applicationList = GetAppList();
            if (applicationList != null)
            {
                foreach (var app in applicationList)
                {
                    if (!_mainView.Data.Contains(app))
                        _mainView.Data.Add(app);
                }
            }
            FillTreeView();
            UpdateDataFromConfig();
        }

        private List<EnviromentModel>? GetAppList()
        {
            var filesList = Directory.GetFiles(_config.AppsDir);
            if (filesList == null)
                return null;

            var list = new List<EnviromentModel>();
            for (var i = 0; i < filesList.Length; i++)
            {
                var envModel = GetEnvModel(filesList[i]);
                if (envModel != null)
                {
                    list.Add(envModel);
                }
            }

            var directoriesList = GetAppListRecursive(new List<string>(), _config.AppsDir);
            if (directoriesList != null)
            {
                foreach (var directory in directoriesList)
                {
                    filesList = Directory.GetFiles(directory);
                    if (filesList != null)
                    {
                        for (var i = 0; i < filesList.Length; i++)
                        {
                            var envModel = GetEnvModel(filesList[i]);
                            if (envModel != null)
                            {
                                list.Add(envModel);
                            }
                        }
                    }
                }
            }

            return list;
        }

        private List<string> GetAppListRecursive(List<string> list, string dirPath)
        {
            try
            {
                foreach (var subDir in Directory.EnumerateDirectories(dirPath))
                {
                    list.Add(subDir);
                    GetAppListRecursive(list, subDir);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                LogHelper.WriteLine(ex, _config, LogginLevel.ERROR);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLine(ex, _config, LogginLevel.ERROR);
            }

            return list;
        }

        private EnviromentModel? GetEnvModel(string filePath)
        {
            try
            {
                string targetPath = FileHelper.GetTargerPath(filePath);
                if(FileHelper.IsExcecutive(targetPath))
                {
                    FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(targetPath);
                    string? envName = GetEnvName(targetPath);
                    string clientName = GetClientName(targetPath);
                    string appName = GetAppName(targetPath);

                    AppModel appModel = new(appName, targetPath);
                    string? version = FileHelper.GetVersionAppFromSCCFile(targetPath);
                    appModel.SetShortName(GetShortName(appName));
                    appModel.GenerateAppVersion(version);

                    EnviromentModel envModel = new(_config, envName ?? Properties.Resources.LocalEnvName, clientName, appModel, null);
                    return envModel;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLine(ex, _config, LogginLevel.ERROR);
            }
            return null;
        }

        private string? GetEnvName(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Directory?.Parent?.Name.StartsWith('Q') == true
                    || fileInfo.Directory?.Parent?.Name.StartsWith('L') == true)
                {
                    return fileInfo.Directory?.Parent?.Name;
                }
                else
                {
                    return fileInfo.Name.Split(' ')[1].Split('@')[0];
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLine(ex, _config, LogginLevel.WARNING);
                return Properties.Resources.LocalEnvName;
            }
        }

        private string GetClientName(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                var clientName = fileInfo.Directory?.Parent?.Parent?.Name;
                if(!string.IsNullOrEmpty(clientName))
                {
                    return clientName;
                }
                else
                {
                    clientName = fileInfo.Name.Split(' ')[1].Split('@')[1];
                    return clientName.Contains('.') ? clientName.Remove(clientName.IndexOf('.')) : clientName;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLine(ex, _config, LogginLevel.WARNING);
                return Properties.Resources.NoClientName;
            }
        }

        private string GetAppName(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                if(fileInfo.Name.Contains('_'))
                {
                    var appName = fileInfo.Name.Split('.')[0];
                    return appName.Split('_')[2];
                }
                else
                {
                    return filePath.Contains(' ') ?
                        Path.GetFileNameWithoutExtension(filePath).Split(' ')[0] ?? Properties.Resources.Unknow
                        : Path.GetFileNameWithoutExtension(filePath) ?? Properties.Resources.Unknow;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLine(ex, _config, LogginLevel.ERROR);
                return Properties.Resources.Unknow;
            }
        }

        private string GetShortName(string appName)
        {
            foreach (var item in _config.UnwantedPartsAppNames)
            {
                if (appName.Contains(item.From))
                {
                    return appName.Replace(item.From, item.To);
                }
            }
            return appName.Contains("32") ? appName.Replace("32", "") : appName;
        }

        private void FillTreeView()
        {
            _mainView.TreeData.Clear();

            var newTree = new ObservableCollection<SimpleNodeModel>();
            if (_envButtonClicked)
            {
                foreach (var item in _mainView.Data)
                {
                    CreateNewNode(newTree, item, item.EnvName, item.ClientName);
                }
            }
            if (_clientsButtonClicked)
            {
                foreach (var item in _mainView.Data)
                {
                    CreateNewNode(newTree, item, item.ClientName, item.EnvName);
                }
            }

            foreach (var node in newTree)
            {
                if (node.Childs?.Count == 1)
                {
                    _mainView.TreeData.Add(new SimpleNodeModel(null,
                        string.Format("{0} | {1}", node.NodeName, node.Childs.First().NodeName),
                        node.Childs.First().Childs));
                }
                else
                {
                    _mainView.TreeData.Add(node);
                }
            }
            ExpandAllNodes(_mainView.TreeData, _mainTreeIsExpandedFlag);
            Application.Current.Dispatcher.Invoke(() =>
            {
                _mainView.OnPropertyChanged(nameof(_mainView.TreeData));
            });
        }

        private void ExpandAllNodes(IEnumerable<SimpleNodeModel> nodes, bool isExpanded)
        {
            foreach (var node in nodes)
            {
                if (node.Childs?.Any() == true)
                {
                    node.IsExpanded = isExpanded;
                    ExpandAllNodes(node.Childs, isExpanded);
                }
            }
        }

        private void CreateNewNode(ObservableCollection<SimpleNodeModel> tree, EnviromentModel item, string nodeName, string subName)
        {
            SimpleNodeModel? node = tree.FirstOrDefault(n => n.NodeName.Equals(nodeName));

            if (node != null && node.Childs?.Any() == true)
            {
                SimpleNodeModel? branch = node.Childs?.FirstOrDefault(n => n.NodeName.Equals(subName));
                if (branch?.Childs?.Any() == true)
                {
                    branch.Childs?.Add(new(branch, item.AppModel.ToString(), item.AppModel.TargetPath));
                }
                else
                {
                    SimpleNodeModel? newBranch = new(node, subName);
                    newBranch.Childs?.Add(new(newBranch, item.AppModel.ToString(), item.AppModel.TargetPath));
                    node.Childs?.Add(newBranch);
                }
            }
            else
            {
                var newNode = new SimpleNodeModel(null, nodeName);
                var newBranch = new SimpleNodeModel(newNode, subName);
                newNode.Childs?.Add(newBranch);
                newBranch.Childs?.Add(new(newBranch, item.AppModel.ToString(), item.AppModel.TargetPath));
                tree.Add(newNode);
            }
        }

        private void UpdateDataFromConfig()
        {
            foreach(var item in _mainView.Data)
            {
                var enviroment = _config.EnvList.FirstOrDefault(q => q.EnvName.Equals(item.EnvName));
                if (enviroment != null && item.WebServiceUrl?.Equals(enviroment.Url) == false)
                {
                    item.UpdateTargetUri(enviroment.Url);
                    item.AppModel.SetWebServiceUrl(enviroment.Url);
                }
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                _mainView.FilteredData.Refresh();
            });
        }

        private void UpdateDataVersion()
        {
            foreach (var item in _mainView.Data)
            {
                if (item.IsActive == true && IsWindowsSystem(item))
                {
                    if(item.GcmWebServiceVersion == null)
                    {
                        var gcmWebserviceUrl = item.WebServiceUrl;
                        string? content = GetWebContent(item.WebServiceUrl);
                        item.GenerateGcmVersion(_config, content);
                    }
                    if(item.AppModel.WebServiceVersion == null)
                    {
                        string? content = GetWebContent(item.AppModel.WebServiceUrl);
                        item.AppModel.GenerateAppVersion(_config, content);
                    }
                }
            }
        }

        private bool IsWindowsSystem(EnviromentModel enviroment)
        {
            return enviroment.System.Equals(_config.EnvPrefix.WindowsPrefix);
        }

        private string? GetWebContent(string? websiteurl)
        {
            string? content = null;
            if(!string.IsNullOrEmpty(websiteurl))
            {
                try
                {
#pragma warning disable SYSLIB0014 // Type or member is obsolete
                    WebClient webClient = new WebClient();
#pragma warning restore SYSLIB0014 // Type or member is obsolete
                    content = webClient.DownloadString(websiteurl);
                }
                catch(Exception ex)
                {
                    LogHelper.WriteLine(ex, _config, LogginLevel.ERROR);
                }
            }
            return content;
        }

        private void TimerTick(object? sender, EventArgs e)
        {
            LogHelper.WriteLine("Timer Tick", _config, LogginLevel.INFO);
            RefreshGUI();
        }

        private void RefreshGUI()
        {
            _mainView.Data.Clear();
            FillData();

            int activeTasks = _pingBackgroundTask.ActiveTasks;
            if (activeTasks > 0)
            {
                _spinAnimation.Stop();
                _pingBackgroundTask.CancelTask();
            }
            else
            {
                _spinAnimation.Begin();
                _ = _pingBackgroundTask.StartAsync();
            }
        }

        private void RunApp(string? targetPath)
        {
            if (string.IsNullOrEmpty(targetPath))
                MessageBox.Show(Properties.Resources.EmptyTargetPath, Properties.Resources.Info, 
                    MessageBoxButton.OK, MessageBoxImage.Hand);
            else
            {
                try
                {
                    OuterAppsManaging outerAppsManaging = new(targetPath);
                    outerAppsManaging.StartApp(out _);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLine(ex, _config, LogginLevel.ERROR);
                }
            }
        }

        public void OpenWebsite(string? websiteUrl)
        {
            if (string.IsNullOrEmpty(websiteUrl) || !Uri.IsWellFormedUriString(websiteUrl, UriKind.Absolute))
                MessageBox.Show(Properties.Resources.EmptyUrl, Properties.Resources.Info, 
                    MessageBoxButton.OK, MessageBoxImage.Hand);
            else
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = websiteUrl,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLine(ex, _config, LogginLevel.ERROR);
                }
            }
        }

        #endregion

    }
}

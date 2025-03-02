using AppSearch.MVC.Helpers;
using AppSearch.MVC.Models;
using IWshRuntimeLibrary;
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
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace AppSearch.MVC.Controllers
{
    public class MainController(MainWindow view)
    {
        private readonly MainWindow _mainView = view;
        private DispatcherTimer _refreshTimer;
        private DispatcherTimer _timer;
        private ConfigurationModel _config;
        private string _mainDirAppPath;
        private bool _envButtonClicked;
        private bool _clientsButtonClicked;
        private bool _mainTreeIsExpandedFlag;
        private uint _timeCounter;
        private Storyboard _spinAnimation;
        private bool _spinAnimationActive;
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

        public void OpenRafiTerm()
        {
            RunApp(_config.UserConfig.RafiTermPath);
        }

        public void OpenSccAppUpdater()
        {
            RunApp(_config.UserConfig.SccUpdaterPath);
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
            _spinAnimation.Stop();
            _spinAnimationActive = false;
            _pingBackgroundTask.CancelTask();

            _spinAnimation.Begin();
            _spinAnimationActive = true;
            _ = _pingBackgroundTask.StartAsync();
            _timeCounter = 0;
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
                if(_mainView.MinHeight == _mainView.Height || (_mainView.Height == _mainView.MinHeight + _mainView.TreeViewGrid.Height))
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
            if(_mainView.DataGrid.SelectedItem is EnviromentModel model && _editor?.Activate() != true)
            {
                _editor = new AppConfigEditor(this, model);
                _editor.Show();
            }
        }

        public void EditUrlsClick()
        {
            if (_editor?.Activate() != true)
            {
                var list = new List<EnviromentModel>();
                foreach(EnviromentModel item in _mainView.Data)
                {
                    if(!list.Any(q => q.EnvName.Equals(item.EnvName)))
                    {
                        list.Add(item);
                    }
                }
                _editor = new AppConfigEditor(this, list.OrderBy(q => q.ClientName).ToList());
                _editor.Show();
            }
        }

        public void AddNewApp()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = Properties.Resources.ChooseFile,
                Filter = string.Format("{0} (*.exe;*.scc)|*.exe;*.scc", Properties.Resources.AllExecutiveFiles),
                Multiselect = false
            };
            if(openFileDialog.ShowDialog() == true)
            {
                FileHelper.CreateShortcut(_config, openFileDialog.FileName);
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

        public bool UpdateData(string envName, string url, out bool savedInConfig)
        {
            bool changed = false;
            savedInConfig = false;
            foreach (var item in _mainView.DataGrid.ItemsSource)
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
                foreach(var item in _config.UserConfig.EnvList)
                {
                    if(item.EnvName.Equals(envName))
                    {
                        item.Url = url;
                        savedInConfig = true;
                    }
                }
                if(!savedInConfig)
                {
                    _config.UserConfig.EnvList.Add(new ConfigurationModel.EnviromentUrl
                    {
                        EnvName = envName,
                        Url = url
                    });
                    savedInConfig = true;
                }
            }
            return changed;
        }

        #region Private Methods

        private void InitializeResources()
        {
            _mainView.Icon = FileHelper.ByteArrayToImageSource(Properties.Resources.Icon);
            _mainView.Title = Properties.Resources.ApplicationName;

            _mainView.SearchLabel.Content = Properties.Resources.SearchLabel;
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
            if (!_config.UserConfig.UnwantedPartsAppNames.Any(item => item.From.Equals(item1.From)))
                _config.UserConfig.UnwantedPartsAppNames.Add(item1);
            if (!_config.UserConfig.UnwantedPartsAppNames.Any(item => item.From.Equals(item2.From)))
                _config.UserConfig.UnwantedPartsAppNames.Add(item2);
            if (!_config.UserConfig.UnwantedPartsAppNames.Any(item => item.From.Equals(item3.From)))
                _config.UserConfig.UnwantedPartsAppNames.Add(item3);
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
                Interval = TimeSpan.FromSeconds(_config.DefaultConfig.RefreshTimerInterval)
            };
            _refreshTimer.Tick += RefreshTimerTick;
            _refreshTimer.Start();

            _timeCounter = 0;
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += TimerTick;
            _timer.Start();

            _pingBackgroundTask = new PingBackgroundTask(_mainView.Data, _config);
            _pingBackgroundTask.TaskCanceled += PingBackgroundTask_TaskCanceled;
            _pingBackgroundTask.TaskCompleted += PingBackgroundTask_TaskCompleted;

            _spinAnimation.Begin();
            _spinAnimationActive = true;
            _ = _pingBackgroundTask.StartAsync();
        }

        private void PingBackgroundTask_TaskCanceled(object sender, object arg)
        {
            if(_config.DefaultConfig.EnableLogging == LogginLevel.INFO)
            {
                MessageBox.Show(GetInfo(Properties.Resources.TaskEndedOnPing, Properties.Resources.TaskEnded,
                nameof(BackgroundTask.TaskCanceled), arg));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                int remainingTasks = _pingBackgroundTask.ActiveTasks;

                if (remainingTasks == 0)
                {
                    UpdateDataVersion();
                    _spinAnimation.Stop();
                    _spinAnimationActive = false;
                }
                _mainView.FilteredData.Refresh();
            });
        }

        private void PingBackgroundTask_TaskCompleted(object sender, object arg)
        {
            if(_config.DefaultConfig.EnableLogging == LogginLevel.INFO)
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
                    _spinAnimationActive = false;
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
                var list = new List<EnviromentModel>();
                foreach (var app in applicationList)
                {
                    if (!list.Contains(app))
                        list.Add(app);
                }
                _mainView.Data = new ObservableCollection<EnviromentModel>(list.OrderBy(item => item.EnvName));
            }
            FillTreeView();
            UpdateDataFromConfig();
        }

        private List<EnviromentModel>? GetAppList()
        {
            var filesList = FileHelper.GetFiles(_config);
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

            var directoriesList = FileHelper.GetAppListRecursive(_config);
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

        private EnviromentModel? GetEnvModel(string filePath)
        {
            try
            {
                string targetPath = filePath;
                if (IsShortCut(filePath))
                {
                    targetPath = GetShortcutTarget(filePath);
                }
                if(IsExcecutive(targetPath) && FileHelper.Exists(targetPath))
                {
                    FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(targetPath);
                    string? envName = GetEnvName(targetPath);

                    string clientName = Properties.Resources.NoClientName;
                    if (!Properties.Resources.LocalEnvName.Equals(envName))
                        clientName = GetClientName(targetPath);

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

        private bool IsShortCut(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".lnk", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsExcecutive(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".exe", StringComparison.OrdinalIgnoreCase)
                || Path.GetExtension(filePath).Equals(".scc", StringComparison.OrdinalIgnoreCase);
        }

        private string GetShortcutTarget(string shortcutPath)
        {
            try
            {
                WshShell shell = new();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                return shortcut.TargetPath;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLine(ex, _config, LogginLevel.ERROR);
            }
            return string.Empty;
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
                    var nameParts = appName.Split('_');
                    return nameParts[nameParts.Length - 1];
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
            foreach (var item in _config.UserConfig.UnwantedPartsAppNames)
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
                var enviroment = _config.UserConfig.EnvList.FirstOrDefault(q => q.EnvName.Equals(item.EnvName));
                if (enviroment != null && item.WebServiceUrl?.Equals(enviroment.Url) == false)
                {
                    item.UpdateTargetUri(enviroment.Url);
                    item.AppModel.SetWebServiceUrl(enviroment.Url);
                }
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                _mainView.FilteredData?.Refresh();
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
            return enviroment.System.Equals(_config.DefaultConfig.EnvPrefix.WindowsPrefix);
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

        private void RefreshTimerTick(object? sender, EventArgs e)
        {
            LogHelper.WriteLine("Timer Tick", _config, LogginLevel.INFO);
            Application.Current.Dispatcher.Invoke(RefreshGUI);
        }

        private void TimerTick(object? sender, EventArgs e)
        {
            _timeCounter++;
            _mainView.Title = string.Format("{0} - {1}:{2} min Since Last Refresh", Properties.Resources.ApplicationName, _timeCounter/60, _timeCounter%60 < 10 ? string.Format("0{0}", _timeCounter % 60) : _timeCounter % 60);
        }

        private void RefreshGUI()
        {
            _mainView.Data = [];
            FillData();

            RestartPingBackgroundTask();
            _timeCounter = 0;
        }

        private void RestartPingBackgroundTask()
        {
            int activeTasks = _pingBackgroundTask.ActiveTasks;
            if (activeTasks > 0 && _spinAnimationActive)
            {
                _spinAnimation.Stop();
                _spinAnimationActive = false;
                _pingBackgroundTask.CancelTask();
            }
            else
            {
                _spinAnimation.Begin();
                _spinAnimationActive = true;
                _ = _pingBackgroundTask.StartAsync();
            }
        }

        private void RunApp(string? targetPath)
        {
            if (string.IsNullOrEmpty(targetPath))
                MessageBox.Show(Properties.Resources.EmptyTargetPath, Properties.Resources.Info, 
                    MessageBoxButton.OK, MessageBoxImage.Hand);
            else if (!FileHelper.Exists(targetPath))
                MessageBox.Show(string.Format(Properties.Resources.FileDoesNotExist, targetPath), Properties.Resources.Info,
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

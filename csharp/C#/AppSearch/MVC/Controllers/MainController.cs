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

namespace AppSearch.MVC.Controllers
{
    internal class MainController(MainWindow view)
    {
        private readonly MainWindow _mainView = view;
        private DispatcherTimer _refreshTimer;
        private ConfigurationModel _config;
        private string _mainDirAppPath;
        private bool _envButtonClicked;
        private bool _clientsButtonClicked;

        public void Initialize()
        {
            InitializeConfig();
            InitializeData();
            InitializeResources();
        }

        public void SearchTextBoxTextChanged()
        {
            _mainView.FilteredData.Refresh();
            FillTreeView();
        }

        public void SearchTextBoxLoaded()
        {
            _mainView.SearchTextBox.Focus();
        }

        public static void DataGridMouseDoubleClick(object sender)
        {
            if (sender is DataGrid data && data.SelectedItem is EnviromentModel row)
            {
                RunApp(row.AppModel.TargetPath);
            }
        }

        public static void TreeViewMouseDoubleClick(object sender)
        {
            if (sender is TreeView treeView && treeView.SelectedItem is SimpleNodeModel node && node.Childs?.Any() == false)
            {
                RunApp(node.TargetPath);
            }
        }

        public void ConfigButtonClick()
        {
            string filePath = Path.Combine(_mainDirAppPath, Properties.Resources.ConfigFileName + ".xml");
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }

        public void VerifyButtonClick()
        {
            foreach(var item in _mainView.Data)
            {
                item.UpdateActive(CheckIfActive(item));
            }
            _mainView.OnPropertyChanged(nameof(_mainView.Data));
        }

        public void RefreshButtonClick()
        {
            _mainView.Data.Clear();
            FillData();
        }

        public void EnvTreeViewClick()
        {
            _envButtonClicked = true;
            _clientsButtonClicked = false;
            _mainView.EnvButtonText.FontWeight = FontWeights.Bold;
            _mainView.ClientsButtonText.FontWeight = FontWeights.Normal;
            FillTreeView();
        }

        public void ClientTreeViewClick()
        {
            _envButtonClicked = false;
            _clientsButtonClicked = true;
            _mainView.EnvButtonText.FontWeight = FontWeights.Normal;
            _mainView.ClientsButtonText.FontWeight = FontWeights.Bold;
            FillTreeView();
        }

        public void ShowTreeViewButtonClick()
        {
            if (_mainView.TreeViewGrid.Visibility == Visibility.Visible)
            {
                _mainView.TreeViewGrid.Visibility = Visibility.Collapsed;
                _mainView.ShowHideTreeViewButton.Content = Properties.Resources.ShowTreeView;
                _mainView.Height -= _mainView.TreeViewGrid.Height;
            }
            else
            {
                _mainView.TreeViewGrid.Visibility = Visibility.Visible;
                _mainView.ShowHideTreeViewButton.Content = Properties.Resources.HideTreeView;
                _mainView.Height += _mainView.TreeViewGrid.Height;
            }
        }

        private void InitializeResources()
        {
            _mainView.Title = Properties.Resources.ApplicationName;

            _mainView.SearchLabel.Content = Properties.Resources.SearchLabel;
            _mainView.ConfigButton.Content = Properties.Resources.EditConfig;
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
                throw new Exception("Error durring cofing reading");
            }
        }

        private void InitializeData()
        {
            _mainView.Data = [];
            _mainView.TreeData = [];

#if DEBUG
            _mainView.Data.Add(new EnviromentModel("Q373", "PATHW", new AppModel("SoftMol", "3.5.73.63", 612637, null), true));
            _mainView.Data.Add(new EnviromentModel("Q373", "PATHW", new AppModel("SoftDxp", "3.5.73.63", 612637, null), true));
            _mainView.Data.Add(new EnviromentModel("Q373", "PATHW", new AppModel("SoftFlw", "3.5.73.63", 612637, null), true));
            _mainView.Data.Add(new EnviromentModel("Q424", "PATHW", new AppModel("SoftMol", "3.5.73.63", 612612, null)));
            _mainView.Data.Add(new EnviromentModel("Q424", "PATHW", new AppModel("SoftDxp", "3.5.73.63", 612612, null)));
            _mainView.Data.Add(new EnviromentModel("Q368", "PATHW", new AppModel("SoftMol", null, null, null), false));
            _mainView.Data.Add(new EnviromentModel("Q368", "PATHW", new AppModel("SoftDxp", null, null, null), false));
#endif

            _envButtonClicked = false;
            _mainView.EnvButtonText.FontWeight = FontWeights.Normal;
            _clientsButtonClicked = true;
            _mainView.ClientsButtonText.FontWeight = FontWeights.Bold;

            FillData();

            _mainView.FilteredData = CollectionViewSource.GetDefaultView(_mainView.Data);
            _mainView.FilteredData.Filter = FilterData;

            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(_config.RefreshTimerInterval)
            };
            _refreshTimer.Tick += TimerTick;
            _refreshTimer.Start();
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

        private void TimerTick(object? sender, EventArgs e)
        {
            Debug.WriteLine("Timer Tick");
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
        }

        private List<EnviromentModel>? GetAppList()
        {
            var filesList = Directory.GetFiles(_config.GetAppsPath());
            if (filesList == null)
                return null;

            var list = new List<EnviromentModel>();
            for (var i = 0; i < filesList.Length - 1; i++)
            {
                var envModel = GetEnvModel(filesList[i]);
                if (envModel != null)
                {
                    list.Add(envModel);
                }
            }

            var directoriesList = Directory.GetDirectories(_config.GetAppsPath());
            if (directoriesList != null)
            {
                for (var i = 0; i < directoriesList.Length - 1; i++)
                {
                    filesList = Directory.GetFiles(_config.GetAppsPath());
                    if (filesList != null)
                    {
                        for (var j = 0; j < filesList.Length - 1; j++)
                        {
                            var envModel = GetEnvModel(filesList[j]);
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
                if(IsExcecutive(targetPath))
                {
                    FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(targetPath);
                    string envName = GetEnvName(targetPath);
                    string clientName = GetClientName(targetPath);
                    AppModel appModel = new(GetAppName(targetPath), GetVersion(fileInfo), GetRevision(fileInfo), targetPath);
                    bool? isActive = CheckIfActive(fileInfo);
                    EnviromentModel envModel = new(envName ?? Properties.Resources.LocalEnvName, clientName, appModel, isActive);
                    return envModel;
                }
            }
            catch (Exception ex)
            {
                if (_config.EnableLogging)
                    Debug.WriteLine(ex);
            }
            return null;
        }

        private static bool IsShortCut(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".lnk", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsExcecutive(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".exe", StringComparison.OrdinalIgnoreCase);
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
                if (_config.EnableLogging)
                    Debug.WriteLine(ex);
            }
            return string.Empty;
        }

        private string GetEnvName(string filePath)
        {
            try
            {
                return (new FileInfo(filePath).Name).Split(' ')[1].Split('@')[0];
            }
            catch (Exception ex)
            {
                if (_config.EnableLogging)
                    Debug.WriteLine(ex);
                return Properties.Resources.LocalEnvName;
            }
        }

        private string GetClientName(string filePath)
        {
            try
            {
                return (new FileInfo(filePath).Name).Split(' ')[1].Split('@')[1];
            }
            catch (Exception ex)
            {
                if (_config.EnableLogging)
                    Debug.WriteLine(ex);
                return Properties.Resources.NoClientName;
            }
        }

        private string GetVersion(FileVersionInfo fileInfo)
        {
            try
            {
                string version = Properties.Resources.Unknow;
                if (fileInfo.ProductVersion != null)
                {
                    if (fileInfo.ProductVersion.Contains('+'))
                    {
                        version = fileInfo.ProductVersion.Substring(0, fileInfo.ProductVersion.IndexOf('+'));
                    }
                    else
                    {
                        version = fileInfo.ProductVersion;
                    }
                }

                return version;
            }
            catch (Exception ex)
            {
                if (_config.EnableLogging)
                    Debug.WriteLine(ex);
                return Properties.Resources.Unknow;
            }
        }

        private string GetAppName(string filePath)
        {
            try
            {
                return Path.GetFileNameWithoutExtension(filePath) ?? Properties.Resources.Unknow;
            }
            catch (Exception ex)
            {
                if (_config.EnableLogging)
                    Debug.WriteLine(ex);
                return Properties.Resources.Unknow;
            }
        }

        private uint? GetRevision(FileVersionInfo fileInfo)
        {
            //TODO
            return null;
        }

        private bool? CheckIfActive(FileVersionInfo fileInfo)
        {
            //TODO
            return null;
        }

        private bool? CheckIfActive(EnviromentModel? item)
        {
            //TODO
            return null;
        }

        private static void RunApp(string? targetPath)
        {
            if (string.IsNullOrEmpty(targetPath))
                MessageBox.Show("Target path is empty");
            else
            {
                OuterAppsManaging outerAppsManaging = new(targetPath);
                outerAppsManaging.StartApp(out _);
            }
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
            ExpandAllNodes(_mainView.TreeData);
            _mainView.OnPropertyChanged(nameof(_mainView.TreeData));
        }

        private static void ExpandAllNodes(IEnumerable<SimpleNodeModel> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.Childs?.Any() == true)
                {
                    node.IsExpanded = true;
                    ExpandAllNodes(node.Childs);
                }
            }
        }

        private static void CreateNewNode(ObservableCollection<SimpleNodeModel> tree, EnviromentModel item, string nodeName, string subName)
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

    }
}

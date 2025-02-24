using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AppSearch.MVC.Controllers;
using AppSearch.MVC.Models;

namespace AppSearch
{
    public partial class MainWindow : Window
    {
        private readonly MainController _controller;

        public ICollectionView FilteredData { get; set; }

        private ObservableCollection<EnviromentModel> _data;
        public ObservableCollection<EnviromentModel> Data 
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        private ObservableCollection<SimpleNodeModel> _treeData;
        public ObservableCollection<SimpleNodeModel> TreeData
        {
            get => _treeData;
            set
            {
                _treeData = value;
                OnPropertyChanged(nameof(TreeData));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            _controller = new MainController(this);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _controller.Initialize();
            DataContext = this;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _controller.SaveConfig();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _controller.SearchBoxValueChanged();
        }

        private void SearchTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            _controller.FocusSearchBox();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _controller.RunApplication();
        }

        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _controller.RunApplicationFromTree(sender);
        }

        private void ConfigButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.ShowConfig();
        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.Verify();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.Refresh();
        }

        private void EnvTreeView_Click(object sender, RoutedEventArgs e)
        {
            _controller.ShowEnviromentTree();
        }

        private void ClientTreeView_Click(object sender, RoutedEventArgs e)
        {
            _controller.ShowClientTree();
        }

        private void ShowTreeViewButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.ShowTree();
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.ExpandTree();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _controller.RunApplication();
        }

        public void OpenSpecificMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _controller.OpenUrl(OpenUrlKind.SPECIFIC);
        }

        public void OpenGcmMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _controller.OpenUrl(OpenUrlKind.GCM);
        }

        public void OpenMomMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _controller.OpenUrl(OpenUrlKind.MOM);
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _controller.EditUrl();
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _controller.DeleteItem();
        }
    }
}
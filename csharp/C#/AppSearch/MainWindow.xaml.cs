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

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _controller.SearchTextBoxTextChanged();
        }

        private void SearchTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            _controller.SearchTextBoxLoaded();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainController.DataGridMouseDoubleClick(sender);
        }

        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainController.TreeViewMouseDoubleClick(sender);
        }

        private void ConfigButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.ConfigButtonClick();
        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.VerifyButtonClick();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.RefreshButtonClick();
        }

        private void EnvTreeView_Click(object sender, RoutedEventArgs e)
        {
            _controller.EnvTreeViewClick();
        }

        private void ClientTreeView_Click(object sender, RoutedEventArgs e)
        {
            _controller.ClientTreeViewClick();
        }

        private void ShowTreeViewButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.ShowTreeViewButtonClick();
        }
    }
}
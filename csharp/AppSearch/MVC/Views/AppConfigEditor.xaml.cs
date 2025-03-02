using AppSearch.MVC.Controllers;
using AppSearch.MVC.Models;
using System.Windows;
using System.Windows.Media;

namespace AppSearch.MVC.Views
{
    public partial class AppConfigEditor : Window
    {
        private readonly MainController _controller;

        private Thickness _defaultThickness;
        private Brush _defaultBrush;

        private static Thickness _errorThickess = new Thickness(2);
        private readonly SolidColorBrush _errorBrush = Brushes.Red;

        public AppConfigEditor(MainController controller, EnviromentModel data)
        {
            InitializeComponent();
            InitializeContent();
            _controller = controller;
            EnvListNames.ItemsSource = new List<EnviromentModel>() { data };
            EnvListNames.SelectedIndex = 0;
            EnvListNames.IsEditable = false;
        }

        public AppConfigEditor(MainController controller, List<EnviromentModel> data)
        {
            InitializeComponent();
            InitializeContent();
            _controller = controller;
            EnvListNames.ItemsSource = data;
        }

        private void InitializeContent()
        {
            EditLabel.Content = Properties.Resources.EditText;
            PathLabel.Content = Properties.Resources.PathName;
            EditPathCheckBox.IsChecked = true;
            HttpsLabel.Content = Properties.Resources.HttpsText;
            EnviromentSystemLabel.Content = Properties.Resources.SystemName;
            WindowsCheckBox.Content = Properties.Resources.WindowsName;
            LinuxCheckBox.Content = Properties.Resources.LinuxName;
            PortLabel.Content = Properties.Resources.PortName;
            SaveButton.Content = Properties.Resources.SaveName;
            CancelButton.Content = Properties.Resources.CancelName;

            _defaultThickness = PathTextBox.BorderThickness;
            _defaultBrush = PathTextBox.BorderBrush;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool updated = false;
            bool savedInConfig = false;
            if (EnvListNames.SelectedItem is EnviromentModel data)
            {
                if (EditPathCheckBox.IsChecked == true && ValidateUrl() == false)
                {
                    PathTextBox.BorderThickness = _errorThickess;
                    PathTextBox.BorderBrush = _errorBrush;
                    MessageBox.Show("Url path is invalid", "Validation", MessageBoxButton.OK, MessageBoxImage.Hand);
                    return;
                }
                if (EditTypePortCheckBox.IsChecked == true)
                {
                    string message = string.Empty;
                    if (ValidateSystem())
                    {
                        WindowsCheckBox.BorderBrush = _errorBrush;
                        WindowsCheckBox.BorderThickness = _errorThickess;
                        LinuxCheckBox.BorderBrush = _errorBrush;
                        LinuxCheckBox.BorderThickness = _errorThickess;
                        message += "Choose the system";
                    }

                    if (ValidatePort() == false)
                    {
                        PortTextBox.BorderThickness = _errorThickess;
                        PortTextBox.BorderBrush = _errorBrush;
                        message += "\n" + "Port is invalid";
                    }
                    MessageBox.Show(message, "Validation", MessageBoxButton.OK, MessageBoxImage.Hand);
                    return;
                }

                if (EditPathCheckBox.IsChecked == true)
                {
                    updated = _controller.UpdateData(data.EnvName, PathTextBox.Text, out savedInConfig);
                }
                else if (EditTypePortCheckBox.IsChecked == true)
                {
                    string gcmUrl = EnviromentModel.GetGcmWebServiceUrl(_controller.Config,
                        HttpsCheckBox.IsChecked == true,
                        GetSystem(_controller.Config),
                        data.EnvName,
                        Int32.Parse(PortTextBox.Text));

                    updated = _controller.UpdateData(data.EnvName, gcmUrl, out savedInConfig);
                }
            }

            if (updated && savedInConfig)
            {
                _controller.SaveConfig();
                if(EnvListNames.Items.Count > 1)
                    MessageBox.Show("Saved", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    Close();
            }
            else
                MessageBox.Show("Nothing Saved", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private string? GetSystem(ConfigurationModel config)
        {
            string? system = null;
            if(WindowsCheckBox.IsChecked == true)
            {
                system = config.DefaultConfig.EnvPrefix.WindowsPrefix;
            }
            if (LinuxCheckBox.IsChecked == true)
            {
                system = config.DefaultConfig.EnvPrefix.LinuxPrefix;
            }
            return system;
        }

        public bool ValidateUrl()
        {
            return !string.IsNullOrWhiteSpace(PathTextBox.Text) && Uri.TryCreate(PathTextBox.Text, UriKind.Absolute, out Uri? uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public bool ValidatePort()
        {
            return !string.IsNullOrWhiteSpace(PortTextBox.Text) 
                && Int32.TryParse(PortTextBox.Text, out int port) 
                && port >= 1024 && port <= 49151;
        }

        private bool ValidateSystem()
        {
            return (WindowsCheckBox.IsChecked == false && LinuxCheckBox.IsChecked == false)
                || (WindowsCheckBox.IsChecked == true && LinuxCheckBox.IsChecked == true);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditPathCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(EditPathCheckBox.IsChecked == true)
            {
                EditTypePortCheckBox.IsChecked = false;
                PathTextBox.IsEnabled = true;
                WindowsCheckBox.IsEnabled = false;
                LinuxCheckBox.IsEnabled = false;
                HttpsCheckBox.IsEnabled = false;
                PortTextBox.IsEnabled = false;

                PathTextBox.BorderThickness = _defaultThickness;
                PathTextBox.BorderBrush = _defaultBrush;
            }
            if(EditPathCheckBox.IsChecked == false)
            {
                PathTextBox.IsEnabled = false;
            }
        }

        private void EditTypePortCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(EditTypePortCheckBox.IsChecked == true)
            {
                EditPathCheckBox.IsChecked = false;
                PathTextBox.IsEnabled = false;
                WindowsCheckBox.IsEnabled = true;
                LinuxCheckBox.IsEnabled = true;
                HttpsCheckBox.IsEnabled = true;
                PortTextBox.IsEnabled = true;

                PortTextBox.BorderThickness = _defaultThickness;
                PortTextBox.BorderBrush = _defaultBrush;
            }
            if (EditTypePortCheckBox.IsChecked == false)
            {
                WindowsCheckBox.IsEnabled = false;
                LinuxCheckBox.IsEnabled = false;
                HttpsCheckBox.IsEnabled = false;
                PortTextBox.IsEnabled = false;
            }
        }

        private void PathTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(PathTextBox.Text) || ValidateUrl() || EditPathCheckBox.IsChecked != true)
            {
                PathTextBox.BorderThickness = _defaultThickness;
                PathTextBox.BorderBrush = _defaultBrush;
            }
            else
            {
                PathTextBox.BorderThickness = _errorThickess;
                PathTextBox.BorderBrush = _errorBrush;
            }
        }

        private void PortTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PortTextBox.Text) || ValidatePort() || EditTypePortCheckBox.IsChecked != true)
            {
                PortTextBox.BorderThickness = _defaultThickness;
                PortTextBox.BorderBrush = _defaultBrush;
            }
            else
            {
                PortTextBox.BorderThickness = _errorThickess;
                PortTextBox.BorderBrush = _errorBrush;
            }
        }

        private void SystemCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(ValidateSystem())
            {
                WindowsCheckBox.BorderBrush = _errorBrush;
                WindowsCheckBox.BorderThickness = _errorThickess;
                LinuxCheckBox.BorderBrush = _errorBrush;
                LinuxCheckBox.BorderThickness = _errorThickess;
            }
            else
            {
                WindowsCheckBox.BorderBrush = _defaultBrush;
                WindowsCheckBox.BorderThickness = _defaultThickness;
                LinuxCheckBox.BorderBrush = _defaultBrush;
                LinuxCheckBox.BorderThickness = _defaultThickness;
            }
        }

        private void EnvListNames_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            PathTextBox.Text = (EnvListNames.SelectedItem as EnviromentModel)?.WebServiceUrl;
            InitializeContent();
        }
    }
}

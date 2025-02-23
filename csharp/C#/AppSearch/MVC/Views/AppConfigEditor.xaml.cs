using AppSearch.MVC.Controllers;
using AppSearch.MVC.Models;
using System.Windows;
using System.Windows.Media;

namespace AppSearch.MVC.Views
{
    public partial class AppConfigEditor : Window
    {
        private readonly EnviromentModel _data;
        private readonly MainController _controller;

        private Thickness _defaultThickness;
        private Brush _defaultBrush;

        private static Thickness _errorThickess = new Thickness(2);
        private readonly SolidColorBrush _errorBrush = Brushes.Red;

        public AppConfigEditor(MainController controller, EnviromentModel data)
        {
            _controller = controller;
            _data = data;
            InitializeComponent();
            InitializeContent();
        }

        private void InitializeContent()
        {
            EditLabel.Content = Properties.Resources.EditText;
            EnvNameEdited.Content = string.Format("Editing: {0}", _data.EnvName);
            PathLabel.Content = Properties.Resources.PathName;
            EditPathCheckBox.IsChecked = true;
            PathTextBox.Text = _data.TargetUri;
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
            if(EditPathCheckBox.IsChecked == true && ValidateUrl() == false)
            {
                PathTextBox.BorderThickness = _errorThickess;
                PathTextBox.BorderBrush = _errorBrush;
                MessageBox.Show("Url path is invalid", "Validation");
                return;
            }
            if(EditTypePortCheckBox.IsChecked == true && ValidatePort() == false)
            {
                PortTextBox.BorderThickness = _errorThickess;
                PortTextBox.BorderBrush = _errorBrush;
                MessageBox.Show("Port is invalid", "Validation");
                return;
            }

            bool updated = false;
            if (EditPathCheckBox.IsChecked == true)
            {
                updated = _controller.UpdateData(_data.EnvName, PathTextBox.Text);
            }
            else if (EditTypePortCheckBox.IsChecked == true)
            {
                updated = _controller.UpdateData(_data.EnvName,
                    _controller.GetWebServiceUrl(_data.EnvName, Int32.Parse(PortTextBox.Text), HttpsCheckBox.IsChecked == true));
            }

            if (updated)
                Close();
            else
                MessageBox.Show("Nothing Saved", "Info");
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
    }
}

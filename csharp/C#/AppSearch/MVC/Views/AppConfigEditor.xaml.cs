using AppSearch.MVC.Controllers;
using AppSearch.MVC.Models;
using System.Windows;

namespace AppSearch.MVC.Views
{
    public partial class AppConfigEditor : Window
    {
        private EnviromentModel _data;
        private MainController _controller;

        public int Port { get; set; }
        public string Url { get; set; }

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
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(EditPathCheckBox.IsChecked == true && ValidateUrl() == false)
            {
                MessageBox.Show("Url path is invalid", "Validation");
                return;
            }
            if(EditTypePortCheckBox.IsChecked == true && ValidatePort() == false)
            {
                MessageBox.Show("Port is invalid", "Validation");
                return;
            }

            bool updated = false;
            if (EditPathCheckBox.IsChecked == true)
            {
                updated = _controller.UpdateData(_data.EnvName, Url);
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
            }
            if (EditTypePortCheckBox.IsChecked == false)
            {
                WindowsCheckBox.IsEnabled = false;
                LinuxCheckBox.IsEnabled = false;
                HttpsCheckBox.IsEnabled = false;
                PortTextBox.IsEnabled = false;
            }
        }
    }
}

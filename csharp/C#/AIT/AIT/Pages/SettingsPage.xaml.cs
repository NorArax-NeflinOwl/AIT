using AIT.Windows;
using System.Windows;
using System.Windows.Controls;

namespace AIT.Pages
{
    public partial class SettingsPage : Page
    {
        private readonly MainWindow m_MainWindow;

        public SettingsPage()
        {
            InitializeComponent();
        }

        public SettingsPage(Window window)
        {
            m_MainWindow = window as MainWindow;
            InitializeComponent();
        }
    }
}

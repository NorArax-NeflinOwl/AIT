using AIT.Windows;
using System.Windows;
using System.Windows.Controls;

namespace AIT.Pages
{
    public partial class DashboardPage : Page
    {
        private readonly MainWindow m_MainWindow;

        public DashboardPage()
        {
            InitializeComponent();
        }

        public DashboardPage(Window window)
        {
            m_MainWindow = window as MainWindow;
            InitializeComponent();
        }
    }
}

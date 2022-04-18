using AIT.Windows;
using System.Windows;
using System.Windows.Controls;

namespace AIT.Pages
{
    public partial class SchedulerPage : Page
    {
        private readonly MainWindow m_MainWindow;

        public SchedulerPage()
        {
            InitializeComponent();
        }

        public SchedulerPage(Window window)
        {
            m_MainWindow = window as MainWindow;
            InitializeComponent();
        }
    }
}

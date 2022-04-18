using AIT.Windows;
using System.Windows;
using System.Windows.Controls;

namespace AIT.Pages
{
    public partial class CalendarPage : Page
    {
        private readonly MainWindow m_MainWindow;

        public CalendarPage()
        {
            InitializeComponent();
        }

        public CalendarPage(Window window)
        {
            m_MainWindow = window as MainWindow;
            InitializeComponent();
        }
    }
}

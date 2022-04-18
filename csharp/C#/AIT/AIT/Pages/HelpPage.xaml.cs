using AIT.Windows;
using System.Windows;
using System.Windows.Controls;

namespace AIT.Pages
{
    public partial class HelpPage : Page
    {
        private readonly MainWindow m_MainWindow;

        public HelpPage()
        {
            InitializeComponent();
        }

        public HelpPage(Window window)
        {
            m_MainWindow = window as MainWindow;
            InitializeComponent();
        }
    }
}

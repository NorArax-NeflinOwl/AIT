using AIT.Windows;
using System.Windows;
using System.Windows.Controls;

namespace AIT.Pages
{
    public partial class VatCalculatorPage : Page
    {
        private readonly MainWindow m_MainWindow;

        public VatCalculatorPage()
        {
            InitializeComponent();
        }

        public VatCalculatorPage(Window window)
        {
            m_MainWindow = window as MainWindow;
            InitializeComponent();
        }
    }
}

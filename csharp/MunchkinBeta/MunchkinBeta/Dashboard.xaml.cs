using MunchkinBeta.Controllers;
using System.Windows.Controls;

namespace MunchkinBeta
{
    /// <summary>
    /// Logika interakcji dla klasy Dashboard.xaml
    /// </summary>
    public partial class DashboarPage : Page
    {
        /// <summary>
        /// Controler miedzy view a modelem
        /// </summary>
        public ViewModel ViewModel { get; }

        /// <summary>
        /// Plansza do gry przez użytkownika
        /// </summary>
        public DashboarPage(ViewModel viewModel)
        {
            InitializeComponent();

            this.ViewModel = viewModel;
        }
    }
}

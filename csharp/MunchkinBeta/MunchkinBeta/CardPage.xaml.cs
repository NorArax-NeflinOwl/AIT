using System.Windows.Controls;
using System.Windows;

namespace MunchkinBeta
{
    /// <summary>
    /// Logika interakcji dla klasy CardPage.xaml
    /// </summary>
    public partial class CardPage : Page
    {
        /// <summary>
        /// 
        /// </summary>
        public CardPage(CardView view)
        {
            InitializeComponent();
            SetView(view);
        }

        private void SetView(CardView view)
        {
            switch(view)
            {
                case CardView.BackDoor:
                    BackDoor.Visibility = Visibility.Visible;
                    BackTreasure.Visibility = Visibility.Hidden;
                    FrontDoor.Visibility = Visibility.Hidden;
                    FrontTreasure.Visibility = Visibility.Hidden;
                    break;
                case CardView.BackTreasure:
                    BackDoor.Visibility = Visibility.Hidden;
                    BackTreasure.Visibility = Visibility.Visible;
                    FrontDoor.Visibility = Visibility.Hidden;
                    FrontTreasure.Visibility = Visibility.Hidden;
                    break;
                case CardView.FrontDoor:
                    BackDoor.Visibility = Visibility.Hidden;
                    BackTreasure.Visibility = Visibility.Hidden;
                    FrontDoor.Visibility = Visibility.Visible;
                    FrontTreasure.Visibility = Visibility.Hidden;
                    break;
                case CardView.FrontTreasure:
                    BackDoor.Visibility = Visibility.Hidden;
                    BackTreasure.Visibility = Visibility.Hidden;
                    FrontDoor.Visibility = Visibility.Hidden;
                    FrontTreasure.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}

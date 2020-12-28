using MunchkinLib.Mediators;
using MunchkinLib.Models;
using System.Windows;
using System.Windows.Controls;

namespace MunchkinBeta
{
    /// <summary>
    /// Logika interakcji dla klasy MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private MainWindow mainMindow;
        private Player mainPlayer;

        /// <summary>
        /// Inictialization of MainPages
        /// </summary>
        public MainPage(MainWindow window, Player player)
        {
            mainMindow = window;
            mainPlayer = player;

            InitializeComponent();

            CardsInHand.ItemsSource = mainPlayer.CardsInHand;
            CardsInGame.ItemsSource = mainPlayer.CardsInGame;
            UpdateTable();
        }

        /// <summary>
        /// Check if game master have some stack on the table and update visibility of stacks
        /// </summary>
        public void UpdateTable()
        {
            if (GameMaster.Instance.TreasureCardStackIsNotEmpty)
            {
                TreasureCardsStackGrid.Visibility = Visibility.Visible;
            }
            else
            {
                TreasureCardsStackGrid.Visibility = Visibility.Hidden;
            }

            if (GameMaster.Instance.DoorCardStackIsNotEmpty)
            {
                DoorCardsStackGrid.Visibility = Visibility.Visible;
            }
            else
            {
                DoorCardsStackGrid.Visibility = Visibility.Hidden;
            }


            if (GameMaster.Instance.RejectedTreasureCardStackIsNotEmpty)
            {
                RejectedTreasureCardsStackGrid.Visibility = Visibility.Visible;
            }
            else
            {
                RejectedTreasureCardsStackGrid.Visibility = Visibility.Hidden;
            }

            if (GameMaster.Instance.RejectedDoorCardStackIsNotEmpty)
            {
                RejectedDoorCardsStackGrid.Visibility = Visibility.Visible;
            }
            else
            {
                RejectedDoorCardsStackGrid.Visibility = Visibility.Hidden;
            }

            if (null != CardsInHand.SelectedItem)
            {
                FirstButton.Content = "Użyj";
                SecondButton.Content = "Sprzedaj Karte";
                ThirdButton.Content = "Test";
            }
            else if(null != CardsInGame.SelectedItem)
            {
                FirstButton.Content = "Schowaj kartę";
                SecondButton.Content = "Test";
                ThirdButton.Content = "Test";
            }
            else
            {
                FirstButton.Content = "Otwórz Drzwi";
                SecondButton.Content = "Test";
                ThirdButton.Content = "Test";
            }
        }

        private void ThirdButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateTable();
        }

        private void SecondButton_Click(object sender, RoutedEventArgs e)
        {
            if("Sprzedaj Karte".Equals(SecondButton.Content) && CheckConditions(CardsInHand.SelectedItem as BaseCard))
            {
                CardTreasure cardTreasure = CardsInHand.SelectedItem as CardTreasure;
                int price = null != cardTreasure ? cardTreasure.Price : GameProperties.MinimalPriceByCard;
                string text = price > 0 ? $"Czy na pewno chcesz sprzedać kartę za {price}?" : "Czy na pewno chcesz oddać kartę?";

                MessageBoxResult result = MessageBox.Show(text, "Sprzedaj Karte", MessageBoxButton.YesNo);
                if (MessageBoxResult.Yes.Equals(result))
                {
                    if (null != cardTreasure)
                    {
                        mainPlayer.Money += price;
                        RemoveCardFromHand(cardTreasure);
                        CloseDetailPanel();
                    }
                    else if (CardsInHand.SelectedItem is BaseCard baseCard)
                    {
                        mainPlayer.Money += GameProperties.MinimalPriceByCard;
                        RemoveCardFromHand(baseCard);
                        CloseDetailPanel();
                    }
                }
            }

            UpdateTable();
        }

        private void OpenDetailPanel(ListView listView)
        {
            BaseCard baseCard = listView.SelectedItem as BaseCard;
            if (null != baseCard)
            {
                DetailTitle.Visibility = Visibility.Visible;
                CloseDetailButton.Visibility = Visibility.Visible;
                CardDetails.Visibility = Visibility.Visible;

                //TODO dodać panel z detalami karty
            }
        }

        private void RemoveCardFromHand(BaseCard card)
        {
            mainPlayer.CardsInHand.Remove(card);
            if (card.IsDoorCard)
            {
                GameMaster.Instance.Add2RejectDoorCard(card);
            }
            else
            {
                GameMaster.Instance.Add2RejectTreasureCard(card);
            }
            CardsInHand.SelectedIndex = -1;
        }

        private void RemoveCardFromHandToTable(BaseCard card)
        {
            mainPlayer.CardsInHand.Remove(card);
            CardsInHand.SelectedIndex = -1;
            mainPlayer.CardsInGame.Add(card);
            CardsInGame.SelectedItem = card;
        }

        private void RemoveCardFromTableToHand(BaseCard card)
        {
            mainPlayer.CardsInGame.Remove(card);
            CardsInGame.SelectedIndex = -1;
            mainPlayer.CardsInHand.Add(card);
            CardsInHand.SelectedItem = card;
        }

        private void FirstButton_Click(object sender, RoutedEventArgs e)
        {
            if ("Otwórz Drzwi".Equals(FirstButton.Content))
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz wziąć otworzyć drzwi?", "Otwórz Drzwi", MessageBoxButton.YesNo);
                if (MessageBoxResult.Yes.Equals(result))
                {
                    BaseCard cardDoor = GameMaster.Instance.GiveOneDoorCard();
                    mainPlayer.CardsInHand.Add(cardDoor);
                    CardsInHand.SelectedItem = cardDoor;
                    OpenDetailPanel(CardsInHand);
                }
            }
            else if ("Użyj".Equals(FirstButton.Content) && CheckConditions(CardsInHand.SelectedItem as BaseCard))
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz użyć karty?", "Użyj", MessageBoxButton.YesNo);
                if (MessageBoxResult.Yes.Equals(result))
                {
                    RemoveCardFromHandToTable(CardsInHand.SelectedItem as BaseCard);
                }
            }
            else if("Schowaj kartę".Equals(FirstButton.Content) && CheckConditions(CardsInGame.SelectedItem as BaseCard))
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz schować kartę?", "Schowaj kartę", MessageBoxButton.YesNo);
                if (MessageBoxResult.Yes.Equals(result))
                {
                    RemoveCardFromTableToHand(CardsInGame.SelectedItem as BaseCard);
                }
            }

            UpdateTable();
        }

        private bool CheckConditions(BaseCard baseCard)
        {
            if (baseCard is CardDoor)// door && door.SpecialEfectHandler())
            {
                return true;
            }
            else if (baseCard is CardTreasure)// treasure && treasure.SpecialEfectHandler())
            {
                return true;
            }
            return false;
        }

        private void CardsInGame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(null != CardsInGame.SelectedItem)
            {
                OpenDetailPanel(CardsInGame);
                CardsInHand.SelectedIndex = -1;
            }
            UpdateTable();
        }

        private void CardsInHand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != CardsInHand.SelectedItem)
            {
                OpenDetailPanel(CardsInHand);
                CardsInGame.SelectedIndex = -1;
            }
            UpdateTable();
        }

        private void MinimalizationButton_Click(object sender, RoutedEventArgs e)
        {
            mainMindow.WindowState = WindowState.Minimized;
            UpdateTable();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            mainMindow.Close();
        }

        private void UnsellectInGameItemButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDetailPanel();
            CardsInGame.SelectedIndex = -1;
            UpdateTable();
        }

        private void UnsellectInHandButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDetailPanel();
            CardsInHand.SelectedIndex = -1;
            UpdateTable();
        }

        private void CloseDetailPanel()
        {
            DetailTitle.Visibility = Visibility.Hidden;
            CloseDetailButton.Visibility = Visibility.Hidden;
            CardDetails.Visibility = Visibility.Hidden;
        }

        private void CloseDetailButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDetailPanel();
            CardsInGame.SelectedIndex = -1;
            CardsInHand.SelectedIndex = -1;
        }
    }
}

using MunchkinBeta.Controls;
using MunchkinLib.Mediators;
using MunchkinLib.Models;
using MunchkinLib.Models.Source;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<ICard> cardsFromStack;

        /// <summary>
        /// Inictialization of MainPages
        /// </summary>
        public MainPage(MainWindow window, Player player)
        {
            mainMindow = window;
            mainPlayer = player;

            cardsFromStack = new List<ICard>();

            InitializeComponent();

            CardsInHand.ItemsSource = mainPlayer.CardsInHand;
            CardsInGame.ItemsSource = mainPlayer.CardsInGame;
            UpdateTable();
            UpdateAtributes();
        }

        private void UpdateTable()
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

            StateManager.SetButtons(this, mainPlayer);
        }

        private void UpdateAtributes()
        {
            PlayerAtributes.Items.Clear();

            var dictionary = mainPlayer.GetAtributeList();

            foreach(var element in dictionary)
            {
                if(!string.IsNullOrWhiteSpace(element.Value))
                    PlayerAtributes.Items.Add(element.Key + ": " + element.Value);
            }
        }

        private void ThirdButton_Click(object sender, RoutedEventArgs e)
        {
            if (MunchkinGlobals.Instance.PlayerActions[Actions.UseCardsFromHand].Equals(ThirdButton.Content))
            {
                UseCardsFromHand();
            }
            else if (MunchkinGlobals.Instance.PlayerActions[Actions.Escape].Equals(ThirdButton.Content))
            {
                Escape();
            }
            else if (MunchkinGlobals.Instance.PlayerActions[Actions.PickUpCard].Equals(ThirdButton.Content))
            {
                FindTreasure();
            }
            else if (MunchkinGlobals.Instance.GeneralPlayerActions[GeneralActions.HideCard].Equals(ThirdButton.Content))
            {
                if (RejectedDoorCardsStackGrid.IsEnabled && RejectedTreasureCardsStackGrid.IsEnabled)
                {
                    HideCardFromStack();
                }
                else
                {
                    // TODO HideCard();
                }
            }
            else if (MunchkinGlobals.Instance.PlayerActions[Actions.EndRound].Equals(ThirdButton.Content))
            {
                EndRound();
            }
            UpdateTable();
            UpdateAtributes();
        }

        private void SecondButton_Click(object sender, RoutedEventArgs e)
        {
            if (MunchkinGlobals.Instance.GeneralPlayerActions[GeneralActions.SellCardForLevel].Equals(SecondButton.Content))
            {
                SellCardForLevel();
            }
            else if (MunchkinGlobals.Instance.PlayerActions[Actions.UseCardsFromHand].Equals(ThirdButton.Content))
            {
                UseCardsFromHand();
            }
            else if (MunchkinGlobals.Instance.PlayerActions[Actions.FindTreasure].Equals(SecondButton.Content))
            {
                FindTreasure();
            }
            else if (MunchkinGlobals.Instance.GeneralPlayerActions[GeneralActions.HideCard].Equals(SecondButton.Content))
            {
                // TODO HideCard();
            }

            UpdateTable();
            UpdateAtributes();
        }

        private void FirstButton_Click(object sender, RoutedEventArgs e)
        {
            if (MunchkinGlobals.Instance.PlayerActions[Actions.StartRound].Equals(FirstButton.Content))
            {
                StartRound();
            }
            else if (MunchkinGlobals.Instance.PlayerActions[Actions.OpenDoor].Equals(FirstButton.Content))
            {
                OpenDoor();
            }
            else if (MunchkinGlobals.Instance.PlayerActions[Actions.Fight].Equals(FirstButton.Content))
            {
                OpenDoor();
            }
            else if (MunchkinGlobals.Instance.PlayerActions[Actions.NextPhase].Equals(FirstButton.Content))
            {
                NextPhase();
            }
            else if (MunchkinGlobals.Instance.PlayerActions[Actions.UseCardsFromHand].Equals(FirstButton.Content))
            {
                UseCardsFromHand();
            }

            UpdateTable();
            UpdateAtributes();
        }

        private void UseCardsFromHand()
        {
            if (CheckConditions(CardsInHand.SelectedItem as ICard))
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz użyć tej karty?", MunchkinGlobals.Instance.PlayerActions[Actions.UseCardsFromHand], MessageBoxButton.YesNo);
                if (MessageBoxResult.Yes.Equals(result))
                {
                    if (ExecuteCardFunction(CardsInHand.SelectedItem as ICard))
                    {
                        RemoveCardFromHandToTable(CardsInHand.SelectedItem as ICard);
                    }
                }
            }
            else
            {
                MessageBox.Show("Nie możesz użyć tej karty!", MunchkinGlobals.Instance.PlayerActions[Actions.UseCardsFromHand], MessageBoxButton.OK);
            }
        }

        private void Escape()
        {
            // TODO zrobić rzut kostką
        }

        private void HideCardFromStack()
        {
            while(cardsFromStack.Count != 0)
            {
                var card = cardsFromStack.Last();
                cardsFromStack.Remove(card);
                RemoveCardFromTableToRejectedStack(card);
            }
            CloseStackDetailPanel();
        }

        private void SellCardForLevel()
        {
            int sum = 0;
            bool res = true;
            var listOfSelectedCards = CardsInHand.SelectedItems as List<ICard>;

            foreach(var card in listOfSelectedCards)
            {
                if(CheckConditions(card))
                {
                    sum += card.IntegerAttributes[CardAttributes.Price];
                }
                else
                {
                    res = false;
                    break;
                }
            }

            if (res && GameProperties.MinimalPriceForNewLevel <= sum + mainPlayer.Money)
            {
                string text = sum > 0 ? $"Czy na pewno chcesz sprzedać kartę/y za {sum}?" : "Czy na pewno chcesz oddać kartę/y?";

                MessageBoxResult result = MessageBox.Show(text, MunchkinGlobals.Instance.GeneralPlayerActions[GeneralActions.SellCardForLevel], MessageBoxButton.YesNo);
                if (MessageBoxResult.Yes.Equals(result))
                {
                    foreach (var card in listOfSelectedCards)
                    {
                        mainPlayer.Money += card.IntegerAttributes[CardAttributes.Price];
                        RemoveCardFromHand(card);
                    }

                    var accession = Math.Floor(Convert.ToDouble(mainPlayer.Money + sum) / GameProperties.MinimalPriceForNewLevel);

                    result = MessageBox.Show($"Zyskałeś {sum} sztuk złota, możesz teraz kupić {accession} poziomów. Czy chcesz teraz je wydać?", 
                        MunchkinGlobals.Instance.GeneralPlayerActions[GeneralActions.SellCardForLevel], 
                        MessageBoxButton.YesNo);

                    if (MessageBoxResult.Yes.Equals(result))
                    {
                        if (CheckMaxLevel(mainPlayer.Money))
                        {
                            mainPlayer.Level += Convert.ToInt32(accession);
                            MessageBox.Show($"Uzkałeś +{accession} poziomu. Poziom = {mainPlayer.Level}", MunchkinGlobals.Instance.GeneralPlayerActions[GeneralActions.SellCardForLevel], MessageBoxButton.OK);
                        }
                        else
                        {
                            MessageBox.Show("Nie możesz kupić ostatniego poziomu tymi kartami!", MunchkinGlobals.Instance.GeneralPlayerActions[GeneralActions.SellCardForLevel], MessageBoxButton.OK);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Nie możesz sprzedać tych kart aby zystac nowy poziom", MunchkinGlobals.Instance.GeneralPlayerActions[GeneralActions.SellCardForLevel], MessageBoxButton.OK);
            }
        }

        private void OpenDoor()
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz otworzyć drzwi?", MunchkinGlobals.Instance.PlayerActions[Actions.OpenDoor], MessageBoxButton.YesNo);
            if (MessageBoxResult.Yes.Equals(result))
            {
                Card cardDoor = GameMaster.Instance.GiveOneDoorCard();
                if(CardTypeFlags.Monster.Equals(cardDoor.CardType))
                {
                    FightWithMonster(cardDoor);
                }
                else
                {
                    mainPlayer.CardsInHand.Add(cardDoor);
                    CardsInHand.SelectedItem = cardDoor;
                    OpenTableDetailPanel(cardDoor);
                }

                StateManager.UpdatePlayerPhase(mainPlayer, Actions.OpenDoor, cardDoor.CardType);
                ClearTable();
            }
        }

        private void FindTreasure()
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz przeszukać pokój?", MunchkinGlobals.Instance.PlayerActions[Actions.FindTreasure], MessageBoxButton.YesNo);
            if (MessageBoxResult.Yes.Equals(result))
            {
                //TODO dodać ilość kart do zabrania z pokoju
                Card cardDoor = GameMaster.Instance.GiveOneTreasureCard();
                mainPlayer.CardsInHand.Add(cardDoor);
                CardsInHand.SelectedItem = cardDoor;
                OpenHandDetailPanel(cardDoor);

                StateManager.UpdatePlayerPhase(mainPlayer, Actions.FindTreasure);
                ClearTable();
            }
        }

        private void StartRound()
        {
            StateManager.UpdatePlayerPhase(mainPlayer, Actions.StartRound);
            ClearTable();
        }

        private void NextPhase()
        {
            StateManager.UpdatePlayerPhase(mainPlayer, Actions.NextPhase);
            ClearTable();
        }

        private void EndRound()
        {
            StateManager.UpdatePlayerPhase(mainPlayer, Actions.EndRound);
            ClearTable();
        }

        private bool CheckMaxLevel(int sum)
        {
            var accession = Math.Floor(Convert.ToDouble(sum) / GameProperties.MinimalPriceForNewLevel);
            return GameProperties.MaxLevel - accession - 1 > mainPlayer.Level;
        }

        private void OpenTableDetailPanel(object obj)
        {
            if (obj is ICard card)
            {
                CardTableDetails.Visibility = Visibility.Visible;

                CardTableDetailsControl.Content = new CardCtrl(card, CardTableDetails);
            }
        }

        private void OpenHandDetailPanel(object obj)
        {
            if (obj is ICard card)
            {
                CardHandDetails.Visibility = Visibility.Visible;

                CardHandDetailsControl.Content = new CardCtrl(card, CardHandDetails);
            }
        }

        private void OpenDetailPanelFromStack(object obj)
        {
            if (obj is ICard card)
            {
                CardFromStack.Visibility = Visibility.Visible;

                CardFromStackControl.Content = new CardCtrl(card, CardFromStack);
            }
        }

        private void AddCardToPanelStack(object obj)
        {

        }

        private void RemoveCardFromHand(ICard card)
        {
            mainPlayer.CardsInHand.Remove(card);
            RemoveCardFromTableToRejectedStack(card);
            CardsInHand.SelectedIndex = -1;
        }

        private void RemoveCardFromTableToRejectedStack(ICard card)
        {
            if (card.CardType.HasFlag(CardTypeFlags.Door))
            {
                GameMaster.Instance.Add2RejectDoorCard(card);
            }
            else
            {
                GameMaster.Instance.Add2RejectTreasureCard(card);
            }
        }

        private void RemoveCardFromHandToTable(ICard card)
        {
            mainPlayer.CardsInHand.Remove(card);
            CardsInHand.SelectedIndex = -1;
            mainPlayer.CardsInGame.Add(card);
            CardsInGame.SelectedItem = card;
        }

        private void RemoveCardFromTableToHand(ICard card)
        {
            mainPlayer.CardsInGame.Remove(card);
            CardsInGame.SelectedIndex = -1;
            mainPlayer.CardsInHand.Add(card);
            CardsInHand.SelectedItem = card;
        }

        private void FightWithMonster(ICard card)
        {
            //TODO
        }

        private void ClearTable()
        {
            while (cardsFromStack.Any())
            {
                var card = cardsFromStack.Last();
                RemoveCardFromTableToRejectedStack(card);
                cardsFromStack.Remove(card);
            }
        }

        private bool CheckConditions(ICard card)
        {
            //TODO CheckSpecialEfectHandler() (dodać obsługe)
            return true;
        }

        private bool ExecuteCardFunction(ICard card)
        {
            return true;
        }

        private void CardsInGame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(null != CardsInGame.SelectedItem)
            {
                OpenTableDetailPanel(CardsInGame.SelectedItem);
                CardsInHand.SelectedIndex = -1;
            }
            UpdateTable();
        }

        private void CardsInHand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != CardsInHand.SelectedItem)
            {
                OpenHandDetailPanel(CardsInHand.SelectedItem);
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
            CardTableDetails.Visibility = Visibility.Hidden;
            CardsInGame.SelectedIndex = -1;
            UpdateTable();
        }

        private void UnsellectInHandButton_Click(object sender, RoutedEventArgs e)
        {
            CardHandDetails.Visibility = Visibility.Hidden;
            CardsInHand.SelectedIndex = -1;
            UpdateTable();
        }

        private void CloseStackDetailPanel()
        {
            CardFromStack.Visibility = Visibility.Hidden;

            while (cardsFromStack.Any())
            {
                var card = cardsFromStack.Last();
                RemoveCardFromTableToRejectedStack(card);
                cardsFromStack.Remove(card);
            }
        }

        private void DoorStack_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz otworzyć drzwi?", "Otwórz Drzwi", MessageBoxButton.YesNo);
            if (MessageBoxResult.Yes.Equals(result))
            {
                ICard card = GameMaster.Instance.GiveOneDoorCard();
                mainPlayer.CardsInHand.Add(card);
                CardsInHand.SelectedItem = card;
                OpenDetailPanelFromStack(card);
            }
        }

        private void TreasureStack_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz wziąć skarb?", "Weź skarb", MessageBoxButton.YesNo);
            if (MessageBoxResult.Yes.Equals(result))
            {
                ICard card = GameMaster.Instance.GiveOneTreasureCard();
                mainPlayer.CardsInHand.Add(card);
                CardsInHand.SelectedItem = card;
                OpenDetailPanelFromStack(card);
            }
        }

        private void RejectedDoorStack_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz zobaczyć odrzuconą kartę drzwi?", "Zobacz drzwi", MessageBoxButton.YesNo);
            if (MessageBoxResult.Yes.Equals(result))
            {
                ICard card = GameMaster.Instance.ShowOneRejectedDoorCard();
                OpenDetailPanelFromStack(card);
            }
        }

        private void RejectedTreasureStack_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz zobaczyć odrzucony skarb?", "Zobacz skarb", MessageBoxButton.YesNo);
            if (MessageBoxResult.Yes.Equals(result))
            {
                ICard card = GameMaster.Instance.ShowOneRejectedDoorCard();
                OpenDetailPanelFromStack(card);
            }
        }
    }
}

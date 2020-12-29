using MunchkinLib.Mediators;
using MunchkinLib.Models;
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
        private Stack<BaseCard> cardsToPick;

        /// <summary>
        /// Inictialization of MainPages
        /// </summary>
        public MainPage(MainWindow window, Player player)
        {
            mainMindow = window;
            mainPlayer = player;

            cardsToPick = new Stack<BaseCard>();

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
            if (MunchkinGlobals.PlayerActions[PlayerActions.UseCardsFromHand].Equals(ThirdButton.Content))
            {
                UseCardsFromHand();
            }
            else if (MunchkinGlobals.PlayerActions[PlayerActions.Escape].Equals(ThirdButton.Content))
            {
                Escape();
            }
            else if (MunchkinGlobals.PlayerActions[PlayerActions.PickUpCard].Equals(ThirdButton.Content))
            {
                PickUpCard();
            }
            else if (MunchkinGlobals.GeneralPlayerActions[GeneralPlayerActions.HideCard].Equals(ThirdButton.Content))
            {
                HideCard();
            }
            else if (MunchkinGlobals.PlayerActions[PlayerActions.EndRound].Equals(ThirdButton.Content))
            {
                EndRound();
            }
            UpdateTable();
            UpdateAtributes();
        }

        private void SecondButton_Click(object sender, RoutedEventArgs e)
        {
            if (MunchkinGlobals.GeneralPlayerActions[GeneralPlayerActions.SellCardForLevel].Equals(ThirdButton.Content))
            {
                SellCardForLevel();
            }
            else if (MunchkinGlobals.PlayerActions[PlayerActions.UseCardsFromHand].Equals(ThirdButton.Content))
            {
                UseCardsFromHand();
            }
            else if (MunchkinGlobals.PlayerActions[PlayerActions.FindTreasure].Equals(ThirdButton.Content))
            {
                FindTreasure();
            }
            else if (MunchkinGlobals.GeneralPlayerActions[GeneralPlayerActions.HideCard].Equals(ThirdButton.Content))
            {
                PickUpCard();
            }

            UpdateTable();
            UpdateAtributes();
        }

        private void FirstButton_Click(object sender, RoutedEventArgs e)
        {
            if (MunchkinGlobals.PlayerActions[PlayerActions.StartRound].Equals(ThirdButton.Content))
            {
                StartRound();
            }
            else if (MunchkinGlobals.PlayerActions[PlayerActions.OpenDoor].Equals(ThirdButton.Content))
            {
                OpenDoor();
            }
            else if (MunchkinGlobals.PlayerActions[PlayerActions.Fight].Equals(ThirdButton.Content))
            {
                OpenDoor();
            }
            else if (MunchkinGlobals.PlayerActions[PlayerActions.NextPhase].Equals(ThirdButton.Content))
            {
                NextPhase();
            }
            else if (MunchkinGlobals.PlayerActions[PlayerActions.UseCardsFromHand].Equals(ThirdButton.Content))
            {
                UseCardsFromHand();
            }

            UpdateTable();
            UpdateAtributes();
        }

        private void UseCardsFromHand()
        {
            if (CheckConditions(CardsInHand.SelectedItem as BaseCard))
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz użyć tej karty?", MunchkinGlobals.PlayerActions[PlayerActions.UseCardsFromHand], MessageBoxButton.YesNo);
                if (MessageBoxResult.Yes.Equals(result))
                {
                    if (ExecuteCardFunction(CardsInHand.SelectedItem as BaseCard))
                    {
                        RemoveCardFromHandToTable(CardsInHand.SelectedItem as BaseCard);
                    }
                }
            }
            else
            {
                MessageBox.Show("Nie możesz użyć tej karty!", MunchkinGlobals.PlayerActions[PlayerActions.UseCardsFromHand], MessageBoxButton.OK);
            }
        }

        private void Escape()
        {
            // TODO zrobić rzut kostką
        }

        private void PickUpCard()
        {
            if(cardsToPick.Any())
            {
                var card = cardsToPick.Pop();
                mainPlayer.CardsInHand.Add(card);
                if(cardsToPick.Any())
                {
                    OpenDetailPanel(cardsToPick.Pop());
                }
            }
        }

        private void HideCard()
        {
            var card = cardsToPick.Pop();
            if(!cardsToPick.Any())
            {
                CloseDetailPanel();
            }
            RemoveCardFromTableToRejectedStack(card);
        }

        private void SellCardForLevel()
        {
            int sum = 0;
            bool res = true;
            var listOfSelectedCards = CardsInHand.SelectedItems as List<BaseCard>;

            foreach(var card in listOfSelectedCards)
            {
                if(CheckConditions(card))
                {
                    CardTreasure cardTreasure = card as CardTreasure;
                    int price = null != cardTreasure ? cardTreasure.Price : GameProperties.MinimalPriceByCard;
                    sum += price;
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

                MessageBoxResult result = MessageBox.Show(text, MunchkinGlobals.GeneralPlayerActions[GeneralPlayerActions.SellCardForLevel], MessageBoxButton.YesNo);
                if (MessageBoxResult.Yes.Equals(result))
                {
                    foreach (var card in listOfSelectedCards)
                    {
                        CardTreasure cardTreasure = card as CardTreasure;
                        int price = null != cardTreasure ? cardTreasure.Price : GameProperties.MinimalPriceByCard;
                        mainPlayer.Money += price;
                        RemoveCardFromHand(card);
                        CloseDetailPanel();
                    }

                    var accession = Math.Floor(Convert.ToDouble(mainPlayer.Money + sum) / GameProperties.MinimalPriceForNewLevel);

                    result = MessageBox.Show($"Zystałeś {sum} sztuk złota, możesz teraz kupić {accession} poziomów. Czy chcesz teraz je wydać?", 
                        MunchkinGlobals.GeneralPlayerActions[GeneralPlayerActions.SellCardForLevel], 
                        MessageBoxButton.YesNo);

                    if (MessageBoxResult.Yes.Equals(result))
                    {
                        if (CheckMaxLevel(mainPlayer.Money))
                        {
                            mainPlayer.Level += Convert.ToInt32(accession);
                            MessageBox.Show($"Uzkałeś +{accession} poziomu. Poziom = {mainPlayer.Level}", MunchkinGlobals.GeneralPlayerActions[GeneralPlayerActions.SellCardForLevel], MessageBoxButton.OK);
                        }
                        else
                        {
                            MessageBox.Show("Nie możesz kupić ostatniego poziomu tymi kartami!", MunchkinGlobals.GeneralPlayerActions[GeneralPlayerActions.SellCardForLevel], MessageBoxButton.OK);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Nie możesz sprzedać tych kart aby zystac nowy poziom", MunchkinGlobals.GeneralPlayerActions[GeneralPlayerActions.SellCardForLevel], MessageBoxButton.OK);
            }
        }

        private void OpenDoor()
        {
            if(CheckPlayerState())
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz otworzyć drzwi?", MunchkinGlobals.PlayerActions[PlayerActions.OpenDoor], MessageBoxButton.YesNo);
                if (MessageBoxResult.Yes.Equals(result))
                {
                    CardDoor cardDoor = GameMaster.Instance.GiveOneDoorCard();
                    mainPlayer.CardsInHand.Add(cardDoor);
                    CardsInHand.SelectedItem = cardDoor;
                    OpenDetailPanel(cardDoor);

                    StateManager.UpdatePlayerPhase(mainPlayer, PlayerActions.OpenDoor, cardDoor.Type);
                }
            }
        }

        private void FindTreasure()
        {

        }

        private void StartRound()
        {

        }

        private void NextPhase()
        {

        }

        private void EndRound()
        {

        }

        private bool CheckMaxLevel(int sum)
        {
            var accession = Math.Floor(Convert.ToDouble(sum) / GameProperties.MinimalPriceForNewLevel);
            return GameProperties.MaxLevel - accession - 1 > mainPlayer.Level;
        }

        private void OpenDetailPanel(object obj)
        {
            if (obj is BaseCard baseCard)
            {
                DetailTitle.Visibility = Visibility.Visible;
                CardDetails.Visibility = Visibility.Visible;

                CardView cardView = baseCard.IsDoorCard ? CardView.FrontDoor : CardView.FrontTreasure;
                CardDetailsControl.Content = new CardControl(cardView, baseCard, CardDetails);
                if(cardsToPick.Any())
                {
                    var card = cardsToPick.Peek();
                    if(!card.Equals(baseCard))
                    {
                        cardsToPick.Push(baseCard);
                    }
                }
                else
                {
                    cardsToPick.Push(baseCard);
                }
            }
        }

        private void RemoveCardFromHand(BaseCard card)
        {
            mainPlayer.CardsInHand.Remove(card);
            RemoveCardFromTableToRejectedStack(card);
            CardsInHand.SelectedIndex = -1;
        }

        private void RemoveCardFromTableToRejectedStack(BaseCard card)
        {
            if (card.IsDoorCard)
            {
                GameMaster.Instance.Add2RejectDoorCard(card);
            }
            else
            {
                GameMaster.Instance.Add2RejectTreasureCard(card);
            }
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


        private bool CheckPlayerState()
        {
            return true;// TODO dodać fazy rozkgrywki (kroki instrukcji)
        }

        private bool CheckConditions(BaseCard baseCard)
        {
            if (baseCard is CardDoor)// door && door.CheckSpecialEfectHandler()) TODO (dodać obsługe każdej klasy/karty drzwi)
            {
                return true;
            }
            else if (baseCard is CardTreasure)// treasure && treasure.CheckSpecialEfectHandler()) TODO (dodać obsługe każdej klasy/karty skarbów)
            {
                return true;
            }
            return false;
        }

        private bool ExecuteCardFunction(BaseCard baseCard)
        {
            return true;
        }

        private void CardsInGame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(null != CardsInGame.SelectedItem)
            {
                cardsToPick.Pop(); // TODO!!! sprawdzać czy na sotle leży karta ze stosu 
                OpenDetailPanel(CardsInGame.SelectedItem);
                CardsInHand.SelectedIndex = -1;
                DetailTitle.Text = "Opis karty";
            }
            UpdateTable();
        }

        private void CardsInHand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != CardsInHand.SelectedItem)
            {
                cardsToPick.Pop(); // TODO!!! sprawdzać czy na sotle leży karta ze stosu 
                OpenDetailPanel(CardsInHand.SelectedItem);
                CardsInGame.SelectedIndex = -1;
                DetailTitle.Text = "Opis karty (zakryty)";
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
            CardDetails.Visibility = Visibility.Hidden;

            while(cardsToPick.Any())
            {
                RemoveCardFromTableToRejectedStack(cardsToPick.Pop());
            }
        }

        private void DoorStack_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz otworzyć drzwi?", "Otwórz Drzwi", MessageBoxButton.YesNo);
            if (MessageBoxResult.Yes.Equals(result))
            {
                BaseCard card = GameMaster.Instance.GiveOneDoorCard();
                mainPlayer.CardsInHand.Add(card);
                CardsInHand.SelectedItem = card;
                OpenDetailPanel(card);
            }
        }

        private void TreasureStack_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz wziąć skarb?", "Weź skarb", MessageBoxButton.YesNo);
            if (MessageBoxResult.Yes.Equals(result))
            {
                BaseCard card = GameMaster.Instance.GiveOneTreasureCard();
                mainPlayer.CardsInHand.Add(card);
                CardsInHand.SelectedItem = card;
                OpenDetailPanel(card);
            }
        }

        private void RejectedDoorStack_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz zobaczyć odrzuconą kartę drzwi?", "Zobacz drzwi", MessageBoxButton.YesNo);
            if (MessageBoxResult.Yes.Equals(result))
            {
                BaseCard card = GameMaster.Instance.ShowOneRejectedDoorCard();
                OpenDetailPanel(card);
            }
        }

        private void RejectedTreasureStack_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz zobaczyć odrzucony skarb?", "Zobacz skarb", MessageBoxButton.YesNo);
            if (MessageBoxResult.Yes.Equals(result))
            {
                BaseCard card = GameMaster.Instance.ShowOneRejectedDoorCard();
                OpenDetailPanel(card);
            }
        }
    }
}

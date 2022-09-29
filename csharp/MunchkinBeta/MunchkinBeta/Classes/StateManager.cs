using MunchkinLib.Models;
using MunchkinLib.Models.Source;
using System;
using System.Windows;

namespace MunchkinBeta
{
    /// <summary>
    /// 
    /// </summary>
    public class StateManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="player"></param>
        public static void SetButtons(MainPage page, Player player)
        {
            switch(player.ActualPhace)
            {
                case QueuePhase.OutOfRound:
                    page.FirstButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.StartRound];
                    page.SecondButton.Content = MunchkinGlobals.Instance.GeneralPlayerActions[GeneralActions.SellCardForLevel];
                    page.ThirdButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.UseCardsFromHand];

                    page.FirstButton.IsEnabled = true;
                    page.SecondButton.IsEnabled = true;
                    page.ThirdButton.IsEnabled = true;

                    page.TreasureCardsStackGrid.IsEnabled = false;
                    page.DoorCardsStackGrid.IsEnabled = true;
                    page.RejectedTreasureCardsStackGrid.IsEnabled = false;
                    page.RejectedDoorCardsStackGrid.IsEnabled = false;

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Visible;
                    break;

                case QueuePhase.OpenDoor:
                    page.FirstButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.OpenDoor];
                    page.SecondButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.UseCardsFromHand];
                    page.ThirdButton.Content = MunchkinGlobals.Instance.GeneralPlayerActions[GeneralActions.HideCard];

                    page.FirstButton.IsEnabled = true;
                    page.SecondButton.IsEnabled = true;
                    page.ThirdButton.IsEnabled = true;

                    page.TreasureCardsStackGrid.IsEnabled = false;
                    page.DoorCardsStackGrid.IsEnabled = true;
                    page.RejectedTreasureCardsStackGrid.IsEnabled = false;
                    page.RejectedDoorCardsStackGrid.IsEnabled = false;

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Visible;
                    break;

                case QueuePhase.FightPhase:
                    page.FirstButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.Fight];
                    page.SecondButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.UseCardsFromHand];
                    page.ThirdButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.Escape];

                    page.FirstButton.IsEnabled = true;
                    page.SecondButton.IsEnabled = true;
                    page.ThirdButton.IsEnabled = true;

                    page.TreasureCardsStackGrid.IsEnabled = false;
                    page.DoorCardsStackGrid.IsEnabled = false;
                    page.RejectedTreasureCardsStackGrid.IsEnabled = false;
                    page.RejectedDoorCardsStackGrid.IsEnabled = false;

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Visible;
                    break;

                case QueuePhase.FindProblem:
                    page.FirstButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.NextPhase];
                    page.SecondButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.FindTreasure];
                    page.ThirdButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.UseCardsFromHand];

                    page.FirstButton.IsEnabled = true;
                    page.SecondButton.IsEnabled = true;
                    page.ThirdButton.IsEnabled = true;

                    page.TreasureCardsStackGrid.IsEnabled = false;
                    page.DoorCardsStackGrid.IsEnabled = false;
                    page.RejectedTreasureCardsStackGrid.IsEnabled = false;
                    page.RejectedDoorCardsStackGrid.IsEnabled = false;

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Visible;
                    break;

                case QueuePhase.FightPhaseAfterFindProblem:
                    page.FirstButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.Fight];
                    page.SecondButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.UseCardsFromHand];
                    page.ThirdButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.Escape];

                    page.FirstButton.IsEnabled = true;
                    page.SecondButton.IsEnabled = true;
                    page.ThirdButton.IsEnabled = true;

                    page.TreasureCardsStackGrid.IsEnabled = false;
                    page.DoorCardsStackGrid.IsEnabled = false;
                    page.RejectedTreasureCardsStackGrid.IsEnabled = false;
                    page.RejectedDoorCardsStackGrid.IsEnabled = false;

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Visible;
                    break;

                case QueuePhase.PickUpTreasure:
                    page.FirstButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.NextPhase];
                    page.SecondButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.UseCardsFromHand];
                    page.ThirdButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.PickUpCard];

                    page.FirstButton.IsEnabled = true;
                    page.SecondButton.IsEnabled = true;
                    page.ThirdButton.IsEnabled = true;

                    page.TreasureCardsStackGrid.IsEnabled = true;
                    page.DoorCardsStackGrid.IsEnabled = false;
                    page.RejectedTreasureCardsStackGrid.IsEnabled = false;
                    page.RejectedDoorCardsStackGrid.IsEnabled = false;

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Visible;
                    break;

                case QueuePhase.GiveCardsForFree:
                    page.FirstButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.UseCardsFromHand];
                    page.SecondButton.Content = MunchkinGlobals.Instance.GeneralPlayerActions[GeneralActions.SellCardForLevel];
                    page.ThirdButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.EndRound];

                    page.FirstButton.IsEnabled = true;
                    page.SecondButton.IsEnabled = true;
                    page.ThirdButton.IsEnabled = true;

                    page.TreasureCardsStackGrid.IsEnabled = false;
                    page.DoorCardsStackGrid.IsEnabled = false;
                    page.RejectedTreasureCardsStackGrid.IsEnabled = false;
                    page.RejectedDoorCardsStackGrid.IsEnabled = false;

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Visible;
                    break;

                case QueuePhase.ShowStack:
                    page.FirstButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.NextPhase];
                    page.SecondButton.Content = MunchkinGlobals.Instance.PlayerActions[Actions.PickUpCard];
                    page.ThirdButton.Content = MunchkinGlobals.Instance.GeneralPlayerActions[GeneralActions.HideCard];

                    page.FirstButton.IsEnabled = true;
                    page.SecondButton.IsEnabled = true;
                    page.ThirdButton.IsEnabled = true;

                    page.TreasureCardsStackGrid.IsEnabled = false;
                    page.DoorCardsStackGrid.IsEnabled = false;
                    page.RejectedTreasureCardsStackGrid.IsEnabled = true;
                    page.RejectedDoorCardsStackGrid.IsEnabled = true;

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Visible;
                    break;
            }
        }

        /// <summary>
        /// Metoda do update fazy gry danego playera
        /// </summary>
        /// <param name="player"></param>
        /// <param name="action"></param>
        /// <param name="type"></param>
        public static void UpdatePlayerPhase(Player player, Actions action, CardTypeFlags type = CardTypeFlags.Unknown)
        {
            var actualPhase = player.ActualPhace;
            switch(actualPhase)
            {
                case QueuePhase.OutOfRound:
                    if(Actions.StartRound.Equals(action))
                    {
                        player.ActualPhace = QueuePhase.OpenDoor;
                    }
                    else if(Actions.UseCardsFromHand.Equals(action))
                    {
                        if (CardTypeFlags.Special.Equals(type))
                        {
                            player.ActualPhace = QueuePhase.ShowStack;
                        }
                    }
                    break;

                case QueuePhase.OpenDoor:
                    if(Actions.OpenDoor.Equals(action))
                    {
                        if (CardTypeFlags.Monster.Equals(type))
                        {
                            player.ActualPhace = QueuePhase.FightPhase;
                        }
                    }
                    else if (Actions.UseCardsFromHand.Equals(action))
                    {
                        if (CardTypeFlags.Monster.Equals(type))
                        {
                            player.ActualPhace = QueuePhase.FindProblem;
                        }
                    }
                    break;

                case QueuePhase.FightPhase:
                    if (Actions.Fight.Equals(action))
                    {
                        if (CardTypeFlags.Unknown.Equals(type)) // znak nie walka została wygrana
                        {
                            player.ActualPhace = QueuePhase.PickUpTreasure;
                        }
                    }
                    else if (Actions.UseCardsFromHand.Equals(action))
                    {
                        if (CardTypeFlags.Unknown.Equals(type)) // znak nie walka została wygrana
                        {
                            player.ActualPhace = QueuePhase.PickUpTreasure;
                        }
                    }
                    else if (Actions.Escape.Equals(action))
                    {
                        player.ActualPhace = QueuePhase.GiveCardsForFree;
                    }
                    break;

                case QueuePhase.FindProblem:
                    if (Actions.NextPhase.Equals(action))
                    {
                        if (CardTypeFlags.Unknown.Equals(type)) // znak że nie walczylem wcześniej wiec moge zajrzeć do pokoju
                        {
                            player.ActualPhace = QueuePhase.PickUpTreasure;
                        }
                    }
                    else if (Actions.FindTreasure.Equals(action))
                    {
                        if(CardTypeFlags.Unknown.Equals(type)) // znak że nie walczylem wcześniej wiec moge zajrzeć do pokoju
                        {
                            player.ActualPhace = QueuePhase.PickUpTreasure;
                        }
                    }
                    else if (Actions.UseCardsFromHand.Equals(action))
                    {
                        if(CardTypeFlags.Monster.Equals(type))
                        {
                            player.ActualPhace = QueuePhase.FightPhaseAfterFindProblem;
                        }
                    }
                    break;

                case QueuePhase.FightPhaseAfterFindProblem:
                    if (Actions.Fight.Equals(action))
                    {
                        if (CardTypeFlags.Unknown.Equals(type)) // znak nie walka została wygrana
                        {
                            player.ActualPhace = QueuePhase.PickUpTreasure;
                        }
                    }
                    else if (Actions.UseCardsFromHand.Equals(action))
                    {
                        if (CardTypeFlags.Unknown.Equals(type)) // znak nie walka została wygrana
                        {
                            player.ActualPhace = QueuePhase.PickUpTreasure;
                        }
                    }
                    else if (Actions.Escape.Equals(action))
                    {
                        player.ActualPhace = QueuePhase.GiveCardsForFree;
                    }
                    break;

                case QueuePhase.PickUpTreasure:
                    if (Actions.NextPhase.Equals(action))
                    {
                        player.ActualPhace = QueuePhase.GiveCardsForFree;
                    }
                    else if (Actions.PickUpCard.Equals(action))
                    {
                        player.ActualPhace = QueuePhase.GiveCardsForFree;
                    }
                    break;

                case QueuePhase.GiveCardsForFree:
                    if (Actions.EndRound.Equals(action))
                    {
                        player.ActualPhace = QueuePhase.OutOfRound;
                    }
                    break;

                case QueuePhase.ShowStack:
                    if (Actions.NextPhase.Equals(action))
                    {
                        player.ActualPhace = QueuePhase.GiveCardsForFree;
                    }
                    break;
            }
        }
    }
}

using MunchkinLib.Models;
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
                    page.FirstButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.StartRound];
                    page.SecondButton.Content = MunchkinGlobals.GeneralPlayerActions[GeneralPlayerActions.SellCardForLevel];
                    page.ThirdButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.UseCardsFromHand];

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Visible;
                    break;

                case QueuePhase.OpenDoor:
                    page.FirstButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.OpenDoor];
                    page.SecondButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.UseCardsFromHand];

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Hidden;
                    break;

                case QueuePhase.FightPhase:
                    page.FirstButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.Fight];
                    page.SecondButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.UseCardsFromHand];
                    page.ThirdButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.Escape];

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Visible;
                    break;

                case QueuePhase.FindProblem:
                    page.FirstButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.NextPhase];
                    page.SecondButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.FindTreasure];
                    page.ThirdButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.UseCardsFromHand];

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Visible;
                    break;

                case QueuePhase.FightPhaseAfterFindProblem:
                    page.FirstButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.Fight];
                    page.SecondButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.UseCardsFromHand];
                    page.ThirdButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.Escape];

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Visible;
                    break;

                case QueuePhase.PickUpTreasure:
                    page.FirstButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.NextPhase];
                    page.SecondButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.UseCardsFromHand];
                    page.ThirdButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.PickUpCard];

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Visible;
                    break;

                case QueuePhase.GiveCardsForFree:
                    page.FirstButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.UseCardsFromHand];
                    page.SecondButton.Content = MunchkinGlobals.GeneralPlayerActions[GeneralPlayerActions.SellCardForLevel];
                    page.ThirdButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.EndRound];

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Visible;
                    break;

                case QueuePhase.ShowStack:
                    page.FirstButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.NextPhase];
                    page.SecondButton.Content = MunchkinGlobals.PlayerActions[PlayerActions.PickUpCard];
                    page.ThirdButton.Content = MunchkinGlobals.GeneralPlayerActions[GeneralPlayerActions.HideCard];

                    page.FirstButton.Visibility = Visibility.Visible;
                    page.SecondButton.Visibility = Visibility.Visible;
                    page.ThirdButton.Visibility = Visibility.Visible;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainPlayer"></param>
        /// <param name="action"></param>
        /// <param name="type"></param>
        public static void UpdatePlayerPhase(Player mainPlayer, PlayerActions action, DoorType type = DoorType.Unknown)
        {
            var actualPhase = mainPlayer.ActualPhace;
            switch(actualPhase)
            {
                case QueuePhase.OutOfRound:
                    if(PlayerActions.StartRound.Equals(action))
                    {
                        mainPlayer.ActualPhace = QueuePhase.OpenDoor;
                    }
                    else if(PlayerActions.UseCardsFromHand.Equals(action))
                    {
                        if (DoorType.Special.Equals(type))
                        {
                            mainPlayer.ActualPhace = QueuePhase.ShowStack;
                        }
                    }
                    break;

                case QueuePhase.OpenDoor:
                    if(PlayerActions.OpenDoor.Equals(action))
                    {
                        if (DoorType.Monster.Equals(type))
                        {
                            mainPlayer.ActualPhace = QueuePhase.FightPhase;
                        }
                    }
                    else if (PlayerActions.UseCardsFromHand.Equals(action))
                    {
                        if (DoorType.Monster.Equals(type))
                        {
                            mainPlayer.ActualPhace = QueuePhase.FindProblem;
                        }
                    }
                    break;

                case QueuePhase.FightPhase:
                    if (PlayerActions.Fight.Equals(action))
                    {
                        if (DoorType.Unknown.Equals(type)) // znak nie walka została wygrana
                        {
                            mainPlayer.ActualPhace = QueuePhase.PickUpTreasure;
                        }
                    }
                    else if (PlayerActions.UseCardsFromHand.Equals(action))
                    {
                        if (DoorType.Unknown.Equals(type)) // znak nie walka została wygrana
                        {
                            mainPlayer.ActualPhace = QueuePhase.PickUpTreasure;
                        }
                    }
                    else if (PlayerActions.Escape.Equals(action))
                    {
                        mainPlayer.ActualPhace = QueuePhase.GiveCardsForFree;
                    }
                    break;

                case QueuePhase.FindProblem:
                    if (PlayerActions.NextPhase.Equals(action))
                    {
                        if (DoorType.Unknown.Equals(type)) // znak że nie walczylem wcześniej wiec moge zajrzeć do pokoju
                        {
                            mainPlayer.ActualPhace = QueuePhase.PickUpTreasure;
                        }
                    }
                    else if (PlayerActions.FindTreasure.Equals(action))
                    {
                        if(DoorType.Unknown.Equals(type)) // znak że nie walczylem wcześniej wiec moge zajrzeć do pokoju
                        {
                            mainPlayer.ActualPhace = QueuePhase.PickUpTreasure;
                        }
                    }
                    else if (PlayerActions.UseCardsFromHand.Equals(action))
                    {
                        if(DoorType.Monster.Equals(type))
                        {
                            mainPlayer.ActualPhace = QueuePhase.FightPhaseAfterFindProblem;
                        }
                    }
                    break;

                case QueuePhase.FightPhaseAfterFindProblem:
                    if (PlayerActions.Fight.Equals(action))
                    {
                        if (DoorType.Unknown.Equals(type)) // znak nie walka została wygrana
                        {
                            mainPlayer.ActualPhace = QueuePhase.PickUpTreasure;
                        }
                    }
                    else if (PlayerActions.UseCardsFromHand.Equals(action))
                    {
                        if (DoorType.Unknown.Equals(type)) // znak nie walka została wygrana
                        {
                            mainPlayer.ActualPhace = QueuePhase.PickUpTreasure;
                        }
                    }
                    else if (PlayerActions.Escape.Equals(action))
                    {
                        mainPlayer.ActualPhace = QueuePhase.GiveCardsForFree;
                    }
                    break;

                case QueuePhase.PickUpTreasure:
                    if (PlayerActions.NextPhase.Equals(action))
                    {
                        mainPlayer.ActualPhace = QueuePhase.GiveCardsForFree;
                    }
                    else if (PlayerActions.PickUpCard.Equals(action))
                    {
                        mainPlayer.ActualPhace = QueuePhase.GiveCardsForFree;
                    }
                    break;

                case QueuePhase.GiveCardsForFree:
                    if (PlayerActions.EndRound.Equals(action))
                    {
                        mainPlayer.ActualPhace = QueuePhase.OutOfRound;
                    }
                    break;

                case QueuePhase.ShowStack:
                    if (PlayerActions.NextPhase.Equals(action))
                    {
                        mainPlayer.ActualPhace = QueuePhase.GiveCardsForFree;
                    }
                    break;
            }
        }
    }
}

using MunchkinLib.Helpers;
using MunchkinLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace MunchkinBeta
{
    /// <summary>
    /// Logika interakcji dla klasy CardControl.xaml
    /// </summary>
    public partial class CardControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public CardControl(CardView view, BaseCard card, Grid grid)
        {
            InitializeComponent();

            this.Height = grid.Height;
            this.Width = grid.Width;

            SetView(view);
            SetCardInfo(card);
        }

        private void SetView(CardView view)
        {
            switch (view)
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

        private void SetCardInfo(BaseCard card)
        {
            if (card is CardDoor doorCard)
            {
                NameFrontDoor.Text = doorCard.Name;
                if (doorCard.Bonus > 0)
                {
                    TopDescFrontDoor.Visibility = Visibility.Visible;
                    TopDescFrontDoor.Text = $"+{doorCard.Bonus} Bonus";
                }
                else if (doorCard.Level > 0)
                {
                    TopDescFrontDoor.Visibility = Visibility.Visible;
                    TopDescFrontDoor.Text = "Poziom " + doorCard.Level;
                }
                else if (doorCard.EscapeBonus > 0)
                {
                    TopDescFrontDoor.Visibility = Visibility.Visible;
                    TopDescFrontDoor.Text = $"+{doorCard.Level} do Ucieczki.";
                }
                else
                {
                    TopDescFrontDoor.Visibility = Visibility.Hidden;
                }

                var descriptions = SepareteDescription(doorCard.Description);
                if (descriptions.Count == 2)
                {
                    CenterDescFrontDoor.Visibility = Visibility.Visible;
                    BottomDescFrontDoor.Visibility = Visibility.Visible;
                    CenterDescFrontDoor.Text = descriptions[0];
                    BottomDescFrontDoor.Text = descriptions[1];
                }
                else
                {
                    CenterDescFrontDoor.Visibility = Visibility.Hidden;
                    BottomDescFrontDoor.Visibility = Visibility.Visible;
                    BottomDescFrontDoor.Text = doorCard.Description;
                }

                var membership = CustomConverter.MembershipCardsToString(doorCard.Type);
                if (doorCard.Reword > 0)
                {
                    RightInfoFronDoor.Visibility = Visibility.Visible;
                    RightInfoFronDoor.Text = doorCard.Reword + " Skarb";
                }
                else if (!string.IsNullOrEmpty(membership))
                {
                    RightInfoFronDoor.Visibility = Visibility.Visible;
                    RightInfoFronDoor.Text = membership;
                }
                else
                {
                    RightInfoFronDoor.Visibility = Visibility.Hidden;
                }

                if (doorCard.AdditionalLevel > 0)
                {
                    LeftInfoFronDoor.Visibility = Visibility.Visible;
                    LeftInfoFronDoor.Text = doorCard.Reword + " Poziomy";
                }
                else
                {
                    LeftInfoFronDoor.Visibility = Visibility.Hidden;
                }

                if (doorCard.IsAdditional)
                {
                    IconFronDoor.Visibility = Visibility.Visible;
                }
                else
                {
                    IconFronDoor.Visibility = Visibility.Hidden;
                }
            }
            else if (card is CardTreasure treasureCard)
            {
                NameFrontDoor.Text = treasureCard.Name;
                if (treasureCard.Bonus > 0)
                {
                    TopDescFrontTreasure.Visibility = Visibility.Visible;
                    TopDescFrontTreasure.Text = $"Bonus +{treasureCard.Bonus}";
                }
                else
                {
                    TopDescFrontTreasure.Visibility = Visibility.Hidden;
                }

                if (!string.IsNullOrWhiteSpace(treasureCard.Requirement))
                {
                    RequirementFrontTreasure.Visibility = Visibility.Visible;
                    RequirementFrontTreasure.Text = treasureCard.Requirement;
                }
                else
                {
                    RequirementFrontTreasure.Visibility = Visibility.Hidden;
                }

                var descriptions = SepareteDescription(treasureCard.Description);
                if (descriptions.Count == 1)
                {
                    CenterDescFrontTreasure.Visibility = Visibility.Visible;
                    BottomDescFrontTreasure.Visibility = Visibility.Visible;
                    BottomDescFrontTreasure.Text = descriptions[0];
                }
                else if (descriptions.Count == 2)
                {
                    CenterDescFrontTreasure.Visibility = Visibility.Visible;
                    BottomDescFrontTreasure.Visibility = Visibility.Visible;
                    CenterDescFrontTreasure.Text = descriptions[0];
                    BottomDescFrontTreasure.Text = descriptions[1];
                }

                if (treasureCard.Price > 0)
                {
                    RightInfoFronTreasure.Visibility = Visibility.Visible;
                    RightInfoFronTreasure.Text = treasureCard.Price + " Sztuk Złota";
                }
                else
                {
                    RightInfoFronTreasure.Visibility = Visibility.Hidden;
                }

                if (treasureCard.IsBig)
                {
                    if (!string.IsNullOrWhiteSpace(treasureCard.ItemType))
                    {
                        LeftUpInfoFronTreasure.Visibility = Visibility.Visible;
                        LeftUpInfoFronTreasure.Text = "Duża";
                        LeftInfoFronTreasure.Visibility = Visibility.Visible;
                        LeftUpInfoFronTreasure.Text = treasureCard.ItemType;
                        LeftInfoFronTreasure.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(treasureCard.ItemType))
                    {
                        LeftInfoFronTreasure.Visibility = Visibility.Visible;
                        LeftInfoFronTreasure.Text = treasureCard.ItemType;
                    }
                    else
                    {
                        LeftInfoFronTreasure.Visibility = Visibility.Hidden;
                    }
                    LeftUpInfoFronTreasure.Visibility = Visibility.Hidden;
                }

                if (treasureCard.IsAdditional)
                {
                    IconFronTreasure.Visibility = Visibility.Visible;
                }
                else
                {
                    IconFronTreasure.Visibility = Visibility.Hidden;
                }
            }
        }


        private List<string> SepareteDescription(string description)
        {
            List<string> results = new List<string>();

            if (description.Contains("\n\n"))
            {
                results = description.Split("\n\n".ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            return results;
        }
    }
}

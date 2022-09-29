using MunchkinLib.Helpers;
using MunchkinLib.Models;
using MunchkinLib.Models.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MunchkinBeta.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy CardControl.xaml
    /// </summary>
    public partial class CardCtrl : UserControl
    {
        private ICard card;
        private CardView view;

        /// <summary>
        /// Konstruktor controlki carty
        /// </summary>
        public CardCtrl(ICard card, Grid grid)
        {
            InitializeComponent();

            this.card = card;
            this.Height = grid.Height;
            this.Width = grid.Width;
            view = card.CardType.HasFlag(CardTypeFlags.Door) ? CardView.FrontDoor : CardView.FrontTreasure; // what about backDoor and backTreasure?

            SetView();
            SetCardInfo();
        }

        private void SetView()
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

        private void SetCardInfo()
        {
            if (card.CardType.HasFlag(CardTypeFlags.Door))
            {
                NameFrontDoor.Text = card.Name;
                if (card.IntegerAttributes[CardAttributes.Bonus] > 0)
                {
                    TopDescFrontDoor.Visibility = Visibility.Visible;
                    TopDescFrontDoor.Text = $"+{card.IntegerAttributes[CardAttributes.Bonus]} Bonus";
                }
                else if (card.IntegerAttributes[CardAttributes.Level] > 0)
                {
                    TopDescFrontDoor.Visibility = Visibility.Visible;
                    TopDescFrontDoor.Text = "Poziom " + card.IntegerAttributes[CardAttributes.Level];
                }
                else if (card.IntegerAttributes[CardAttributes.EscapeBonus] > 0)
                {
                    TopDescFrontDoor.Visibility = Visibility.Visible;
                    TopDescFrontDoor.Text = $"+{card.IntegerAttributes[CardAttributes.EscapeBonus]} do Ucieczki.";
                }
                else
                {
                    TopDescFrontDoor.Visibility = Visibility.Hidden;
                }

                var descriptions = SepareteDescription(card.Description);
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
                    BottomDescFrontDoor.Text = card.Description;
                }

                var membership = CustomConverter.MembershipCardsToString(card.CardType);
                if (card.IntegerAttributes[CardAttributes.Reword] > 0)
                {
                    RightInfoFronDoor.Visibility = Visibility.Visible;
                    RightInfoFronDoor.Text = card.IntegerAttributes[CardAttributes.Reword] + " Skarb";
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

                if (card.IntegerAttributes[CardAttributes.LevelReword] > 0)
                {
                    LeftInfoFronDoor.Visibility = Visibility.Visible;
                    LeftInfoFronDoor.Text = card.IntegerAttributes[CardAttributes.LevelReword] + " Poziomy";
                }
                else
                {
                    LeftInfoFronDoor.Visibility = Visibility.Hidden;
                }

                if (card.IsAdditional)
                {
                    IconFronDoor.Visibility = Visibility.Visible;
                }
                else
                {
                    IconFronDoor.Visibility = Visibility.Hidden;
                }
            }
            else if (card.CardType.HasFlag(CardTypeFlags.Treasure))
            {
                NameFrontDoor.Text = card.Name;
                if (card.IntegerAttributes[CardAttributes.Bonus] > 0)
                {
                    TopDescFrontTreasure.Visibility = Visibility.Visible;
                    TopDescFrontTreasure.Text = $"Bonus +{card.IntegerAttributes[CardAttributes.Bonus]}";
                }
                else
                {
                    TopDescFrontTreasure.Visibility = Visibility.Hidden;
                }

                if (!string.IsNullOrWhiteSpace(card.StringAttributes[CardAttributes.Requirement]))
                {
                    RequirementFrontTreasure.Visibility = Visibility.Visible;
                    RequirementFrontTreasure.Text = card.StringAttributes[CardAttributes.Requirement];
                }
                else
                {
                    RequirementFrontTreasure.Visibility = Visibility.Hidden;
                }

                var descriptions = SepareteDescription(card.Description);
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

                if (card.IntegerAttributes[CardAttributes.Price] > 0)
                {
                    RightInfoFronTreasure.Visibility = Visibility.Visible;
                    RightInfoFronTreasure.Text = card.IntegerAttributes[CardAttributes.Price] + MunchkinGlobals.Instance.CardsInfoParts[CardFlags.HasPrice];
                }
                else
                {
                    RightInfoFronTreasure.Visibility = Visibility.Hidden;
                }

                var item = CustomConverter.GetCardStringWeaponType(card.CardFlags);
                if (card.CardFlags.HasFlag(CardFlags.IsBig))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        LeftUpInfoFronTreasure.Visibility = Visibility.Visible;
                        LeftUpInfoFronTreasure.Text = MunchkinGlobals.Instance.CardsInfoParts[CardFlags.IsBig];
                        LeftInfoFronTreasure.Visibility = Visibility.Visible;
                        LeftUpInfoFronTreasure.Text = item;
                        LeftInfoFronTreasure.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        LeftInfoFronTreasure.Visibility = Visibility.Visible;
                        LeftInfoFronTreasure.Text = item;
                    }
                    else
                    {
                        LeftInfoFronTreasure.Visibility = Visibility.Hidden;
                    }
                    LeftUpInfoFronTreasure.Visibility = Visibility.Hidden;
                }

                if (card.IsAdditional)
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

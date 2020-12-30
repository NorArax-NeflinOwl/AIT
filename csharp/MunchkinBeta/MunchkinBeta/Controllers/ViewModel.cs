using MunchkinBeta.Controls;
using MunchkinLib.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MunchkinBeta.Controllers
{
    /// <summary>
    /// View model do kontroli dashboardu.
    /// </summary>
    public class ViewModel
    {
        /// <summary>
        /// Główny obiekt gracza
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// Lista wyświetlanych kart w ręce gracza
        /// </summary>
        public ObservableCollection<CardListItemCtrl> CardsInHand { get; set; }

        /// <summary>
        /// Lista wyświetlanych przedmiotów przed grzeczem
        /// </summary>
        public ObservableCollection<CardListItemCtrl> CardsOnGame { get; set; }

        /// <summary>
        /// Stos ze skarbami
        /// </summary>
        public ObservableCollection<CardListItemCtrl> TreasureInRooms { get; set; }

        /// <summary>
        /// Stos z drzwiami
        /// </summary>
        public ObservableCollection<CardListItemCtrl> DoorOfRooms { get; set; }

        /// <summary>
        /// Stos ze skarbami wykorzystanymi
        /// </summary>
        public ObservableCollection<CardListItemCtrl> RejectedTreasureInRooms { get; set; }

        /// <summary>
        /// Stos z drzwiami wykorzystanymi
        /// </summary>
        public ObservableCollection<CardListItemCtrl> RejectedDoorOfRooms { get; set; }

        /// <summary>
        /// Pierwszy programowalny przycisk na planszy
        /// </summary>
        public Button FirstButton { get; set; }

        /// <summary>
        /// Drugi programowalny przycisk na planszy
        /// </summary>
        public Button SecondButton { get; set; }

        /// <summary>
        /// Trzeci programowalny przycisk na planszy
        /// </summary>
        public Button ThirdButton { get; set; }
    }
}

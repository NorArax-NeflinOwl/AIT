namespace MunchkinLib.Models
{
    public interface BaseCard
    {
        bool IsDoorCard
        {
            get;
        }

        string Name
        {
            get;
            set;
        }

        string Description
        {
            get;
            set;
        }

        bool IsAdditional
        {
            get;
            set;
        }

        int Bonus
        {
            get;
            set;
        }

        int HiddenBonus
        {
            get;
            set;
        }

        int EscapeBonus
        {
            get;
            set;
        }

        int HiddenEscapeBonus
        {
            get;
            set;
        }

        bool HasSpecialEfect
        {
            get;
            set;
        }
    }
}

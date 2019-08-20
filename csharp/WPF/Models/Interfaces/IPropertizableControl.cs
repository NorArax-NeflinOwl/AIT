namespace WPF.Models.Interfaces
{
    public interface IPropertizableControl
    {
        IProperties Properties { get; }
        void Init();
        void Subscribe();
    }
}

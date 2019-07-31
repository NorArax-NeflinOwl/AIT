namespace WPF.Models.Interfaces
{
    public interface IPropertizableWindow
    {
        IWindowsProperties Properties { get; }
        void Init();
        void Subscribe();
    }
}

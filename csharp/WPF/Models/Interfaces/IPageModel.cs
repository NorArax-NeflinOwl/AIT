namespace WPF.Models
{
    public interface IPageModel
    {
        string Header { get; set; }
        object Content { get; set; }
    }
}

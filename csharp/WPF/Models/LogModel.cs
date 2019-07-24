using WPF.Enums;

namespace WPF.Models
{
    public class LogModel
    {
        public FileTypeEnum Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}

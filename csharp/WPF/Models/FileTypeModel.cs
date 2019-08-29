using WPF.Models.Enums;

namespace WPF.Models
{
    public class FileTypeModel
    {
        public FileTypesEnum EnumType { get; set; }
        public string StringType { get; set; }
        public PermitionAccountEnum PermitionLevel { get; set; }
        public bool AllowToEmptyContent { get; set; }
    }
}

using WPF.Models.Enums;

namespace WPF.Models
{
    public class FileTypeModel
    {
        public FileTypesEnum EnumType { get; set; }
        public string StringType { get; set; }
        public PermitionAccountEnum PermitionLevel { get; set; }
        public bool AllowToEmptyContent { get; set; }
        public bool AllowToCreate { get; set; }

        public FileTypeModel()
        {
            AllowToCreate = true;
        }

        public override string ToString()
        {
            return StringType;
        }
    }
}

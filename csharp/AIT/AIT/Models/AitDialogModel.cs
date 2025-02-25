using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AIT.Models
{
    public enum DialogType
    {
        ERROR,
        INFORMATION,
        QUESTION
    }

    public class AitDialogModel
    {
        public DialogType Type { get; private set; }
        public string Title { get; private set; }
        public Image Image { get; private set; }
        public string Message { get; private set; }
        public bool OneButtonPanel { get; private set; }

        public AitDialogModel(DialogType type, string message)
        {
            Init(type);
            Type = type;
            Message = message;
        }

        private void Init(DialogType type)
        {
            switch (type)
            {
                default:
                case DialogType.ERROR:
                    Title = "Error";
                    Image = new Image();
                    Image.Source = GetImageSource(Properties.Resources.ErrorImage);//new BitmapImage(new Uri(@"Assets/error.png"));
                    OneButtonPanel = true;
                    break;
                case DialogType.INFORMATION:
                    Title = "Information";
                    Image = new Image();
                    Image.Source = GetImageSource(Properties.Resources.InformationImage);
                    OneButtonPanel = true;
                    break;
                case DialogType.QUESTION:
                    Title = "Question";
                    Image = new Image();
                    Image.Source = GetImageSource(Properties.Resources.QuestionImage);
                    OneButtonPanel = false;
                    break;
            }
        }

        private BitmapImage GetImageSource(byte[] imageObject)
        {
            BitmapImage bitmap = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(imageObject))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze();
            }
            return bitmap;
        }
    }
}

using System.Windows;
using AITLib.Helpers;

namespace AIT.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AitFileManager.Init();
        }
    }
}

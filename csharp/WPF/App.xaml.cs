using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPF.Contexts;
using WPF.Enums;
using WPF.Interfaces;
using WPF.Windows;

namespace WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AitMainContext context = AitMainContext.Instance;
            context.Init();
            context.StartThreadingProccess();

            AitExtendedWindowInterface extWindow = context.GetWindow(AitWindowsEnum.START);
            MainWindow = extWindow.Window;
            if (extWindow != null)
            {
                AitExtendedPageInterface extPage = context.getPage(AitPagesEnum.START);
                if (extPage != null)
                {
                    extWindow.Build(extPage);
                }
                extWindow.Window.Show();
            }
        }
    }
}

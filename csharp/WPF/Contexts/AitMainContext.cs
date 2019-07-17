using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Interfaces;
using WPF.Pages;
using WPF.Windows;

namespace WPF.Contexts
{
    public class AitMainContext
    {
        private AitMainContext()
        {
            pageTrace = new List<AitBasicPageInterface>();
            pages = new Dictionary<AitPagesEnum, AitBasicPageInterface>();
            windows = new Dictionary<AitWindowsEnum, AitBasicWindowInterface>();
        }
        private static object locker = new object();
        private static AitMainContext instance = new AitMainContext();

        public static AitMainContext Instance
        {
            get 
            {
                lock (locker)
                {
                    return instance;
                }
            }
        }

        private List<AitBasicPageInterface> pageTrace;
        private Dictionary<AitPagesEnum, AitBasicPageInterface> pages;
        private Dictionary<AitWindowsEnum, AitBasicWindowInterface> windows;

        public void Init()
        {
            pages.Add(AitPagesEnum.START, new StartPage());
            pages.Add(AitPagesEnum.LOGIN, new LoginPage());
            pages.Add(AitPagesEnum.REGISTRATION, new RegistationPage());
            pages.Add(AitPagesEnum.DASHBOARD, new DashboardPage());
            pages.Add(AitPagesEnum.NOTE, new NotePage());
            pages.Add(AitPagesEnum.SHEDULER, new ShedulerPage());
            pages.Add(AitPagesEnum.WORKOUT, new WorkoutPage());
            pages.Add(AitPagesEnum.FINANSE, new FinansePage());
            pages.Add(AitPagesEnum.COMPANY, new CompanyPage());

            windows.Add(AitWindowsEnum.START, new StartWindow());
            windows.Add(AitWindowsEnum.LOGIN, new LoginWindow());
            windows.Add(AitWindowsEnum.REGISTRATION, new RegistrationWindow());
            windows.Add(AitWindowsEnum.MAIN, new MainWindow());
        }

        public AitBasicPageInterface OpenPageIn(AitPagesEnum pageName, AitWindowsEnum windowName)
        {
            if (pages.ContainsKey(pageName))
            {
                AitBasicPageInterface page = pages[pageName];
                //windows
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPF.Enums;
using WPF.Interfaces;
using WPF.Managers;
using WPF.Models;
using WPF.Pages;
using WPF.Windows;

namespace WPF.Contexts
{
    public class AitMainContext
    {
        /* X.x.x.x.x - major release version
         * x.X.x.x.x - relese version
         * x.x.X.x.x - service pack version
         * x.x.x.X.x - patch version
         * x.x.x.x.X - debug version */

        private static object locker = new object();
        private static AitMainContext instance = new AitMainContext();

        private IList<AitWindowsEnum> pageTrace;

        private IDictionary<AitPagesEnum, AitExtendedPageInterface> pages;
        private IDictionary<AitWindowsEnum, AitExtendedWindowInterface> windows;
        private IDictionary<AitThreadEnum, AitExtendedThreadInterface> threads;

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

        private AitMainContext()
        {
            pageTrace = new List<AitWindowsEnum>();
            pages = new Dictionary<AitPagesEnum, AitExtendedPageInterface>();
            windows = new Dictionary<AitWindowsEnum, AitExtendedWindowInterface>();
            threads = new Dictionary<AitThreadEnum, AitExtendedThreadInterface>();
        }

        public void Init()
        {
            PagesInitialize();
            WindowsInitialize();
            ThreadInitialize();
        }

        public void StartThreadingProccess()
        {
            foreach (var thread in threads)
            {
                thread.Value.Start();
            }
        }

        private void PagesInitialize()
        {
            pages.Add(AitPagesEnum.START, new AitExtendedPageModel(AitPagesEnum.START));
            pages.Add(AitPagesEnum.LOGIN, new AitExtendedPageModel(AitPagesEnum.LOGIN));
            pages.Add(AitPagesEnum.REGISTRATION, new AitExtendedPageModel(AitPagesEnum.REGISTRATION));
            pages.Add(AitPagesEnum.DASHBOARD, new AitExtendedPageModel(AitPagesEnum.DASHBOARD));
            pages.Add(AitPagesEnum.NOTE, new AitExtendedPageModel(AitPagesEnum.NOTE));
            pages.Add(AitPagesEnum.SHEDULER, new AitExtendedPageModel(AitPagesEnum.SHEDULER));
            pages.Add(AitPagesEnum.WORKOUT, new AitExtendedPageModel(AitPagesEnum.WORKOUT));
            pages.Add(AitPagesEnum.FINANSE, new AitExtendedPageModel(AitPagesEnum.FINANSE));
            pages.Add(AitPagesEnum.COMPANY, new AitExtendedPageModel(AitPagesEnum.COMPANY));
        }

        private void WindowsInitialize()
        {
            windows.Add(AitWindowsEnum.START, new AitExtendedWindowsModel(AitWindowsEnum.START));
            windows.Add(AitWindowsEnum.LOGIN, new AitExtendedWindowsModel(AitWindowsEnum.LOGIN));
            windows.Add(AitWindowsEnum.REGISTRATION, new AitExtendedWindowsModel(AitWindowsEnum.REGISTRATION));
            windows.Add(AitWindowsEnum.MAIN, new AitExtendedWindowsModel(AitWindowsEnum.MAIN));
        }

        private void ThreadInitialize()
        {

        }

        public AitExtendedWindowInterface GetWindow(AitWindowsEnum windowName)
        {
            return windows[windowName];
        }

        public AitExtendedPageInterface getPage(AitPagesEnum pageName)
        {
            return pages[pageName];
        }
    }
}

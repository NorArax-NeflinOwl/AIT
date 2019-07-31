using System.Collections.Generic;
using System.Windows.Controls;
using WPF.Models;
using WPF.Models.Enums;
using WPF.UI.Controls;
using WPF.UI.Pages;
using WPF.UI.Pages.Properties;

namespace WPF.Managers
{
    public class TabControlManager
    {
        private IPageModel dashboardPage;
        private TabControl tabControl;
        private Dictionary<string, Button> cache;

        public TabControlManager(TabControl tabControl)
        {
            this.tabControl = tabControl;
            cache = new Dictionary<string, Button>();
        }

        public void Add(IPageModel page)
        {
            var found = false;
            foreach(TabItem item in tabControl.Items)
            {
                if(item.Header.Equals(page.Header))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                cache[page.Header.Header.Text] = page.Header.CloseButton;
                cache[page.Header.Header.Text].Click += CloseButton_Click;
                TabItem item = new TabItem();
                item.Header = page.Header; //TODO
                item.Content = new Frame
                {
                    Content = page.Content
                };
                tabControl.Items.Add(item);
            }

            if (page is DashboardProperties)
                dashboardPage = page;

            LogManager.Instance.LogToFile(new LogInfoModel
            {
                Type = FileTypesEnum.TRACE,
                Message = $"Open new page {page.Header}"
            });
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var header = (((sender as Button)?.Parent as Grid)?.Parent as TabItemHeaderControl);
            Remove(header);
        }

        public void Remove(TabItemHeaderControl header)
        {
            TabItem tabItem = null;
            foreach(TabItem item in tabControl.Items)
            {
                if (item.Header.Equals(header))
                {
                    tabItem = item;
                    break;
                }
            }

            if(tabItem != null)
            {
                LogManager.Instance.LogToFile(new LogInfoModel
                {
                    Type = FileTypesEnum.TRACE,
                    Message = $"Close page {header.Header.Text}"
                });

                tabControl.Items.Remove(tabItem);
                cache[header.Header.Text].Click -= CloseButton_Click;
                cache[header.Header.Text] = null;

                if (tabControl.Items.IsEmpty)
                    Add(dashboardPage);
            }
        }

        public void Clear()
        {
            LogManager.Instance.LogToFile(new LogInfoModel
            {
                Type = FileTypesEnum.TRACE,
                Message = $"Clear MainTabControl"
            });

            tabControl.Items.Clear();
            Add(dashboardPage);
        }
    }
}

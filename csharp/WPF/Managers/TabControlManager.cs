using System.Windows.Controls;
using WPF.Models;
using WPF.Models.Enums;
using WPF.Models.Extensions;
using WPF.UI.Controls;
using WPF.UI.Pages.Properties;

namespace WPF.Managers
{
    public class TabControlManager
    {
        private readonly TabControl tabControl;

        public TabControl TabControl { get => tabControl; }

        public TabControlManager(TabControl tabControl)
        {
            this.tabControl = tabControl;
        }

        public void Add(IPageModel page)
        {
            var mainWindow = WindowsDictionary.GetMainWindow();
            if(mainWindow != null)
            {
                mainWindow.StartLoad(2);
            }

            var index = 0;
            var found = false;
            foreach(TabItem item in tabControl.Items)
            {
                var ctrl = item.Header as TabItemHeaderControl;
                if (ctrl != null && ctrl.Header.Text.Equals(page.Header?.Header.Text))
                {
                    found = true;
                    break;
                }
                index++;
            }

            var message = "{0} page {1}";
            var task = string.Empty;
            if (!found)
            {
                page.Header.CloseButton.Click += CloseButton_Click;
                TabItem item = new TabItem
                {
                    Header = page.Header,
                    Content = new Frame
                    {
                        Content = page.Content
                    }
                };
                tabControl.Items.Add(item);

                tabControl.SelectedIndex = tabControl.Items.Count - 1;
                task = "Open new";
            }
            else
            {
                tabControl.SelectedIndex = index;
                task = "Set focus on";
            }

            LogManager.Instance.LogToFile(new LogInfoModel
            {
                Type = FileTypesEnum.TRACE,
                Message = string.Format(message, task, page.Header.Header.Text)
            });

            if (mainWindow != null)
            {
                mainWindow.EndLoad();
            }
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Button button)
                button.Click -= CloseButton_Click;

            var header = ((sender as Button)?.Parent as Grid)?.Parent as TabItemHeaderControl;
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

                if (tabControl.Items.IsEmpty)
                    Add(new DashboardProperties());
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

            Add(new DashboardProperties());
        }
    }
}

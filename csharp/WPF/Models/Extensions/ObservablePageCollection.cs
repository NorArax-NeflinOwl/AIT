using System.Collections.ObjectModel;
using System.Linq;
using WPF.Managers;
using WPF.Pages;

namespace WPF.Models.Extensions
{
    public class ObservablePageCollection : ObservableCollection<IPageModel>
    {
        private IPageModel dashboardPage;

        public void Add(IPageModel page)
        {
            if (!Contains(page))
                base.Add(page);

            if (page is DashboardPage)
                dashboardPage = page;

            LogManager.Instance.LogToFile(new LogInfoModel
            {
                Type = Enums.FileTypesEnum.TRACE,
                Message = $"Open new page {page.Header}"
            });
        }

        public void Remove(IPageModel page)
        {
            LogManager.Instance.LogToFile(new LogInfoModel
            {
                Type = Enums.FileTypesEnum.TRACE,
                Message = $"Close page {page.Header}"
            });

            base.Remove(page);

            if (!this.Any())
                base.Add(dashboardPage);
        }

        public void Clear()
        {
            LogManager.Instance.LogToFile(new LogInfoModel
            {
                Type = Enums.FileTypesEnum.TRACE,
                Message = $"Clear MainTabControl"
            });

            base.Clear();
            base.Add(dashboardPage);
        }
    }
}

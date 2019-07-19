using System.Diagnostics;
using System.Linq;
using System.Windows;
using WPF.Contexts;
using WPF.Enums;
using WPF.Models;

namespace WPF.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Test();
        }

        private async void Test()
        {
            using (var dataContext = new DBContext())
            {
                dataContext.Accounts.Add(new AccountModel()
                {
                    ID = "AIT-ACC-0000000",
                    Login = "admin",
                    Permition = PermitionAccount.ADMIN,
                    Active = true
                });
                await dataContext.SaveChangesAsync();

                foreach (var acc in dataContext.Accounts.ToList())
                {
                    Debug.WriteLine($"Account ID= {acc.ID}, Account login = {acc.Login}");
                }
            }
        }
    }
}

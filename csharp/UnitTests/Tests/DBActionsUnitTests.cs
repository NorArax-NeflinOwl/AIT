using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using WPF.Contexts;
using WPF.Enums;
using WPF.Models;

namespace UnitTests.Tests
{
    [TestClass]
    public class DBActionsUnitTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var dataContext = new DBContext())
            {
                //dataContext.ReCreate();

                var id = "AIT-ACC-0000001";
                if (!dataContext.Accounts.Any(q => q.ID.Equals(id)))
                {
                    dataContext.Accounts.Add(new AitAccountModel()
                    {
                        ID = id,
                        Login = "manager",
                        Permition = PermitionAccount.MANAGER,
                        IsActive = true
                    });
                    dataContext.SaveChanges();
                }
                Assert.IsTrue(dataContext.Accounts.Any(q => q.ID.Equals(id)));
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            using(var dataContext = new DBContext())
            {
                dataContext.Accounts.ToList().ForEach(q => Debug.WriteLine($"DATE:{DateTime.Now.ToString("dd.MM.yy HH:mm:ss")} ID:{q.ID}"));
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            using (var dbContext = new DBContext())
            {
                var acc = dbContext.Accounts.Where(q => q.ID.Equals("AIT-ACC-0000001")).FirstOrDefault();
                acc.IsActive = false;
                dbContext.SaveChanges();
            }
        }
    }
}

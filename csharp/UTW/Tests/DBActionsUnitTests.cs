using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using WPF.Databases.Contexts;
using WPF.Databases.Models;

namespace UTW.Tests
{
    [TestClass]
    public class DBActionsUnitTests
    {
        [TestMethod]
        public void InsertTest()
        {
            using (var dataContext = PDBContext.Instance.Context)
            {
                dataContext.ReCreate();

                var id = "AIT-ACC-0000001";
                if (!dataContext.Accounts.Any(q => q.ID.Equals(id)) && !dataContext.Stsgenids.Any(q => q.ID.Equals(id)))
                {
                    var acc = new AitAccountModel(dataContext)
                    {
                        ID = id,
                        Login = "test"
                    };
                    acc.Add();
                    acc.Context.SaveChanges();
                }
                Assert.IsTrue(dataContext.Accounts.Any(q => q.ID.Equals(id)));
            }
        }

        [TestMethod]
        public void SelectTest()
        {
            using(var dataContext = PDBContext.Instance.Context)
            {
                dataContext.Accounts.ToList().ForEach(q => Debug.WriteLine($"DATE:{DateTime.Now.ToString("dd.MM.yy HH:mm:ss")} ID:{q.ID}"));
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            using (var dbContext = PDBContext.Instance.Context)
            {
                var acc = dbContext.Accounts.Where(q => q.ID.Equals("AIT-ACC-0000001")).FirstOrDefault();
                if(acc != null)
                {
                    acc.IsActive = false;
                    acc.Update();
                    dbContext.SaveChanges();
                    Assert.IsFalse(dbContext.Accounts.Where(q => q.ID.Equals("AIT-ACC-0000001")).FirstOrDefault().IsActive);
                }
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            using (var dbContext = PDBContext.Instance.Context)
            {
                var acc = dbContext.Accounts.Where(q => q.ID.Equals("AIT-ACC-0000001")).FirstOrDefault();
                if(acc != null)
                {
                    dbContext.Remove(acc);
                }
                var sts = dbContext.Stsgenids.Where(q => q.ID.Equals("AIT-ACC-0000001")).FirstOrDefault();

                Assert.IsNotNull(sts);
                Assert.IsTrue(sts.Delete != null);
            }
        }
    }
}

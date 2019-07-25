using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Enums;
using WPF.Managers.Helpers;

namespace UTW.Tests
{
    [TestClass]
    public class DBActionsUnitTests
    {
        private static readonly string id = Generator.IDGenerator(IDInerfixEnum.ACC);

        [TestMethod]
        public void InsertTest()
        {
            using (var dataContext = PDBContext.Instance.Context)
            {
                //dataContext.ReCreate();

                if (!dataContext.Accounts.Any(q => q.ID.Equals(id)) && !dataContext.Stsgenids.Any(q => q.ID.Equals(id)))
                {
                    var acc = new AitAccountModel(dataContext)
                    {
                        ID = id,
                        Login = "test"
                    };
                    acc.Insert();
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
                var acc = dbContext.Accounts.Where(q => q.ID.Equals(id)).FirstOrDefault();
                if(acc != null)
                {
                    acc.IsActive = true;
                    acc.Update();
                    dbContext.SaveChanges();
                    Assert.IsTrue(dbContext.Accounts.Where(q => q.ID.Equals(id)).FirstOrDefault().IsActive);
                }
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            using (var dbContext = PDBContext.Instance.Context)
            {
                var acc = dbContext.Accounts.Where(q => q.ID.Equals(id)).FirstOrDefault();
                if(acc != null)
                {
                    acc.Delete();
                }
                var sts = dbContext.Stsgenids.Where(q => q.ID.Equals(id)).FirstOrDefault();

                Assert.IsNotNull(sts);
                Assert.IsTrue(sts.Delete != null);

                dbContext.SaveChanges();
            }
        }
    }
}

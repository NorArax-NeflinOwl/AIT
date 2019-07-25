using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Models.Enums;
using WPF.Managers.Helpers;

namespace UTW.UnitTests
{
    [TestClass]
    public class DBActionsUnitTests
    {
        private static readonly string id = Generators.IDGenerator(IDInerfixEnum.ACC);

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
                    dataContext.SaveChanges();
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
                var acc = dbContext.Accounts.Find(id);
                if(acc != null)
                {
                    acc.IsActive = true;
                    acc.Update();
                    dbContext.SaveChanges();
                    Assert.IsTrue(dbContext.Accounts.Find(id).IsActive);
                }
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            using (var dbContext = PDBContext.Instance.Context)
            {
                var acc = dbContext.Accounts.Find(id);
                if(acc != null)
                {
                    acc.Delete();
                }
                var sts = dbContext.Stsgenids.Find(id);

                Assert.IsNotNull(sts);
                Assert.IsTrue(sts.Delete != null);

                dbContext.SaveChanges();
            }
        }
    }
}

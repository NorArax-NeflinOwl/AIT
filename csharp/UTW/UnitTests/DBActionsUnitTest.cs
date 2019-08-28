using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using WPF.Databases.Contexts;
using WPF.Databases.Models;
using WPF.Models.Enums;
using WPF.Managers.Helpers;
using System.Collections.Generic;
using WPF.Managers;
using System.Configuration;

namespace UTW.UnitTests
{
    [TestClass]
    public class DBActionsUnitTests
    {
        private static readonly string id = Generators.RecordIDGenerator(TableInerfixEnum.ACC, false);

        [TestMethod]
        public void InsertTest()
        {
            using (var dataContext = PDBContext.Instance.Context)
            {
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

        [TestMethod]
        public void CreateBigTest()
        {
            using(var context = PDBContext.Instance.Context)
            {
                AitAccountModel account = new AitAccountModel(context)
                {
                    ID = Generators.RecordIDGenerator(TableInerfixEnum.ACC, false),
                    Login = "noraraxneflinowl",
                    Email = "pudwel.n.patryk@gmail.com",
                    Password = Generators.GenerateSha256Hash("S1mplep@ssw0rd"),
                    IsActive = true
                };
                AitUserDataModel userData = new AitUserDataModel(context)
                {
                    ID = Generators.RecordIDGenerator(TableInerfixEnum.USD, false),
                    AssignedTo = account.ID,
                    FirstName = "Patryk",
                    MiddleName = "Norbert",
                    LastName = "Pudwel",
                    Birthday = DateTime.Parse("1995.07.27"),
                    Nick = "NorArax NeflinOwl"
                };
                AitUserHostModel userHostModel = new AitUserHostModel(context)
                {
                    ID = Generators.RecordIDGenerator(TableInerfixEnum.USH, false),
                    AssignedTo = account.ID,
                    HostName = HardwareManager.GetComputerName()
                };
                IList<AitFileModel> files = new List<AitFileModel>
                {
                    new AitFileModel(context)
                    {
                        ID = Generators.RecordIDGenerator(TableInerfixEnum.FLS, false),
                        Name = "Empty Test File 1",
                        FileCreator = account,
                        Type = FileTypesEnum.NOTE
                    },
                    new AitFileModel(context)
                    {
                        ID = Generators.RecordIDGenerator(TableInerfixEnum.FLS, false),
                        Name = "Empty Test File 2",
                        FileCreator = account,
                        Type = FileTypesEnum.TASK
                    }
                };

                account.Insert();
                userData.Insert();
                userHostModel.Insert();
                foreach (var file in files)
                    file.Insert();

                context.SaveChanges();

                account.FillObject();
                Assert.IsNotNull(account.UserData);

                foreach (var file in account.Files)
                    Assert.IsNotNull(file);

                foreach (var host in account.UserHosts)
                    Assert.IsNotNull(host);
            }
        }

        [TestMethod]
        public void CreateManagerTest()
        {
            using (var context = PDBContext.Instance.Context)
            {
                var managerID = ConfigurationManager.AppSettings["TasksManager"].ToString();
                var manager = context.Accounts.Find(managerID);
                if (manager == null)
                {
                    manager = new AitAccountModel(context)
                    {
                        ID = managerID,
                        Login = "taskmanager",
                        Email = ConfigurationManager.AppSettings["AppEmail"].ToString(),
                        Permition = PermitionAccountEnum.MANAGER,
                        IsActive = false
                    };
                    manager.Insert();
                    context.SaveChanges();
                }
            }
        }
    }
}

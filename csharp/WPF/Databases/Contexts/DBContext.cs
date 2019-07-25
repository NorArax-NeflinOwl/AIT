using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Reflection;
using WPF.Databases.Models;
using WPF.Managers;

namespace WPF.Databases.Contexts
{
    public class DBContext : DbContext
    {
        private readonly string databasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);

        public DBContext()
        {
        }

        public void ReCreate()
        {
            try
            {
                Database.EnsureDeleted();
            }
            catch (Exception e)
            {
                LogManager.Instance.LogExceptionToFile(e);
            }

            try
            {
                Database.EnsureCreated();
            }
            catch (Exception e)
            {
                LogManager.Instance.LogExceptionToFile(e);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            var path = $"Data Source={databasePath}\\Databases\\nano.db";
            optionbuilder.UseSqlite(path);
        }

        public DbSet<SysStsgenids> Stsgenids { get; set; }

        public DbSet<AitAccountModel> Accounts { get; set; }

        public DbSet<AitFilesModel> Files { get; set; }

        public DbSet<AitUserDataModel> UsersDatas { get; set; }

        public DbSet<AitUserHostModel> UsersHosts { get; set; }
    }
}

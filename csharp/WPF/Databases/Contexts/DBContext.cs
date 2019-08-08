using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WPF.Databases.Models;
using WPF.Managers;
using WPF.Managers.Helpers;

namespace WPF.Databases.Contexts
{
    public class DBContext : DbContext
    {
        private readonly string databasePath = Environment.CurrentDirectory + "\\Databases";
        private readonly string databaseName = "nano.db";

        public DBContext()
        {
            CreateIfNotExist();
        }

        private void CreateIfNotExist()
        {
            if(!File.Exists(Path.Combine(databasePath, databaseName)))
            {
                FileManager.CreateDBFile(databasePath, databaseName);

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
        }

        public override int SaveChanges()
        {
            Generators.ClearLocalIDs();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            Generators.ClearLocalIDs();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            var path = $"Data Source={databasePath}\\{databaseName}";
            optionbuilder.UseSqlite(path);
        }

        public DbSet<SysStsgenids> Stsgenids { get; set; }

        public DbSet<AitAccountModel> Accounts { get; set; }

        public DbSet<AitFilesModel> Files { get; set; }

        public DbSet<AitUserDataModel> UsersDatas { get; set; }

        public DbSet<AitUserHostModel> UsersHosts { get; set; }
    }
}

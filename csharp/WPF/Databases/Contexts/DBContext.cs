using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WPF.Databases.Models;
using WPF.Managers;
using WPF.Managers.Helpers;
using WPF.Models.Interfaces;

namespace WPF.Databases.Contexts
{
    public class DBContext : DbContext, IDisposableExtended
    {
        private readonly string databasePath = Environment.CurrentDirectory + "\\Databases";
        private readonly string databaseName = "nano.db";

        private static string dbPath;
        public bool IsDisposed { get; set; }

        public DBContext(string path = "")
        {
            if(!string.IsNullOrEmpty(path))
                dbPath = path;
            else
                CreateIfNotExist();
        }

        private void CreateIfNotExist()
        {
            if(!File.Exists(Path.Combine(databasePath, databaseName)))
            {
                FileManager.CreateDBFile(databasePath, databaseName);

                try
                {
                    Database.EnsureCreated();
                }
                catch (Exception e)
                {
                    LogManager.Instance.LogExceptionToFileAndDB(e);
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
            optionbuilder.EnableSensitiveDataLogging();

            if (!string.IsNullOrEmpty(dbPath))
                optionbuilder.UseSqlite($"Data Source={dbPath}");
            else
                optionbuilder.UseSqlite($"Data Source={databasePath}\\{databaseName}");
        }

        public override void Dispose()
        {
            IsDisposed = true;
            base.Dispose();
        }

        public DbSet<SysStsgenids> Stsgenids { get; set; }
        public DbSet<AitAccountModel> AccountsModel { get; set; }
        public DbSet<AitFileModel> FilesModel { get; set; }
        public DbSet<AitUserDataModel> UsersDatasModel { get; set; }
        public DbSet<AitUserHostModel> UsersHostsModel { get; set; }
        public DbSet<AitHostDataModel> HostDatasModel { get; set; }

        public List<AitAccountModel> Accounts
        {
            get
            {
                var deleteEntityMode = ConfigurationManager.AppSettings["DeleteEntryMode"].ToString();
                if (deleteEntityMode.Equals("ON"))
                    return AccountsModel.ToList();
                else
                    return AccountsModel.Where(entry => !entry.IsDeleted).ToList();
            }
        }

        public List<AitFileModel> Files
        {
            get
            {
                var deleteEntityMode = ConfigurationManager.AppSettings["DeleteEntryMode"].ToString();
                if (deleteEntityMode.Equals("ON"))
                    return FilesModel.ToList();
                else
                    return FilesModel.Where(entry => !entry.IsDeleted).ToList();
            }
        }

        public List<AitUserDataModel> UsersDatas
        {
            get
            {
                var deleteEntityMode = ConfigurationManager.AppSettings["DeleteEntryMode"].ToString();
                if (deleteEntityMode.Equals("ON"))
                    return UsersDatasModel.ToList();
                else
                    return UsersDatasModel.Where(entry => !entry.IsDeleted).ToList();
            }
        }

        public List<AitUserHostModel> UsersHosts
        {
            get
            {
                var deleteEntityMode = ConfigurationManager.AppSettings["DeleteEntryMode"].ToString();
                if (deleteEntityMode.Equals("ON"))
                    return UsersHostsModel.ToList();
                else
                    return UsersHostsModel.Where(entry => !entry.IsDeleted).ToList();
            }
        }

        public List<AitHostDataModel> HostDatas
        {
            get
            {
                var deleteEntityMode = ConfigurationManager.AppSettings["DeleteEntryMode"].ToString();
                if (deleteEntityMode.Equals("ON"))
                    return HostDatasModel.ToList();
                else
                    return HostDatasModel.Where(entry => !entry.IsDeleted).ToList();
            }
        }
    }
}

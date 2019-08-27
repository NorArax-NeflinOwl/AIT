using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
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
            optionbuilder.EnableSensitiveDataLogging();

            if (!string.IsNullOrEmpty(dbPath))
                optionbuilder.UseSqlite($"Data Source={dbPath}");
            else
                optionbuilder.UseSqlite($"Data Source={databasePath}\\{databaseName}");
        }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AitUserDataModel>()
                .HasOne(q => q.AccountData)
                .WithOne(p => p.UserData)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AitUserHostModel>()
                .HasOne(q => q.AccountData)
                .WithMany(p => p.UserHosts)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AitFilesModel>()
                .HasOne(q => q.FileCreator)
                .WithMany(p => p.Files)
                .OnDelete(DeleteBehavior.Cascade);
        }*/

        public override void Dispose()
        {
            IsDisposed = true;
            base.Dispose();
        }

        public DbSet<SysStsgenids> Stsgenids { get; set; }

        public DbSet<AitAccountModel> Accounts { get; set; }

        public DbSet<AitFileModel> Files { get; set; }

        public DbSet<AitUserDataModel> UsersDatas { get; set; }

        public DbSet<AitUserHostModel> UsersHosts { get; set; }
    }
}

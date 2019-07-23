using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using WPF.Models;

namespace WPF.Contexts
{
    public class DBContext : DbContext
    {
        private readonly string databasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);

        public DBContext()
        {
        }

        public void ReCreate()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
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

using Microsoft.EntityFrameworkCore;
using WPF.Models;

namespace WPF.Contexts
{
    public class DBContext : DbContext
    {
        public DBContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite(@"Data Source=D:\AIT\sqlite\Sample.db");
        }

        public DbSet<AccountModel> Accounts { get; set; }
    }
}

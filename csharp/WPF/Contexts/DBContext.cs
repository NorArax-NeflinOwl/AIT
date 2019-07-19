using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using WPF.Managers;
using WPF.Models;

namespace WPF.Contexts
{
    public class DBContext : DbContext
    {
        public DBContext()
        {
            if(!Database.EnsureCreated())
                Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite(@"Data Source=D:\AIT\sqlite\Sample.db");
        }

        public DbSet<AccountModel> Accounts { get; set; }
    }
}

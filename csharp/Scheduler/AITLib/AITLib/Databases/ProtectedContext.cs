using AITLib.Helpers;
using AITLib.Models;
using System.Data.Entity;

namespace AITLib.Databases
{
    public class PrivateContext : DbContext
    {
        public virtual DbSet<User> Users { get; }
        public virtual DbSet<UserInfo> UsersInfo { get; }
        public virtual DbSet<Note> Notes { get; }
        public virtual DbSet<Comment> Comments { get; }
        public virtual DbSet<Mission> Missions { get; }
        //public virtual DbSet<Callendar> Callendars { get; }
        //public virtual DbSet<Setting> Settings { get; }

        public PrivateContext() : base("DBConnection")
        {
            AitFileManager.Init();
        }
    }
}

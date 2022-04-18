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
        public virtual DbSet<Callendar> Callendars { get; }
        public virtual DbSet<Setting> Settings { get; }

        public PrivateContext() : base("DBConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<User>().Property(q => q.ID).IsRequired();
            modelBuilder.Entity<User>().Property(q => q.Login).IsRequired();
            modelBuilder.Entity<User>().Property(q => q.PasswordHash).IsRequired();
            modelBuilder.Entity<User>().Property(q => q.CreatedDate).IsRequired();

            modelBuilder.Entity<UserInfo>().Property(q => q.ParentID).IsRequired();
            modelBuilder.Entity<Note>().Property(q => q.ParentID).IsRequired();
            modelBuilder.Entity<Mission>().Property(q => q.ParentID).IsRequired();
            modelBuilder.Entity<Comment>().Property(q => q.ParentID).IsRequired();
            modelBuilder.Entity<Callendar>().Property(q => q.ParentID).IsRequired();
            modelBuilder.Entity<Setting>().Property(q => q.ParentID).IsRequired();*/

            base.OnModelCreating(modelBuilder);
        }
    }
}

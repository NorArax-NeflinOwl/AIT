using AIT.DataBases.DBModel;
using System.Data.Entity;

namespace AIT.DataBases
{
    public class AitDBContext : DbContext
    {
        public virtual DbSet<AitPerson> AitPersons { get; set; }
        public virtual DbSet<AitPersonsDetail> AitPersonsDetails { get; set; }
        public virtual DbSet<AitQuickNote> AitQuickNotes { get; set; }

        public AitDBContext() : base("AitDBConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AitPerson>()
                .HasOptional(s => s.PersonalDetails)
                .WithRequired(ad => ad.Person);

            modelBuilder.Entity<AitQuickNote>()
                .HasRequired(q => q.Person)
                .WithMany(p => p.QuickNotes)
                .HasForeignKey(q => q.PersonID);

            modelBuilder.Entity<AitPerson>()
                .HasMany(p => p.QuickNotes)
                .WithRequired(q => q.Person)
                .WillCascadeOnDelete();

            base.OnModelCreating(modelBuilder);
        }
    }
}

namespace IndiaEntertainers.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class IEDBContext : DbContext
    {
        public IEDBContext()
            : base("name=IEDBContext")
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<tbl_Category> tbl_Category { get; set; }
        public virtual DbSet<tbl_City> tbl_City { get; set; }
        public virtual DbSet<tbl_Entertainer> tbl_Entertainer { get; set; }
        public virtual DbSet<tbl_EntertainerCats> tbl_EntertainerCats { get; set; }
        public virtual DbSet<tbl_EntnMessages> tbl_EntnMessages { get; set; }
        public virtual DbSet<tbl_EntrImages> tbl_EntrImages { get; set; }
        public virtual DbSet<tbl_EntrVideos> tbl_EntrVideos { get; set; }
        public virtual DbSet<tbl_Projectography> tbl_Projectography { get; set; }
        public virtual DbSet<tbl_State> tbl_State { get; set; }
        public virtual DbSet<tbl_SubCategory> tbl_SubCategory { get; set; }
        public virtual DbSet<tbl_TalentSeeker> tbl_TalentSeeker { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<tbl_Category>()
                .HasMany(e => e.tbl_SubCategory)
                .WithRequired(e => e.tbl_Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Entertainer>()
                .HasMany(e => e.tbl_EntertainerCats)
                .WithRequired(e => e.tbl_Entertainer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Entertainer>()
                .HasMany(e => e.tbl_EntrImages)
                .WithRequired(e => e.tbl_Entertainer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Entertainer>()
                .HasMany(e => e.tbl_EntrVideos)
                .WithRequired(e => e.tbl_Entertainer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Entertainer>()
                .HasMany(e => e.tbl_Projectography)
                .WithRequired(e => e.tbl_Entertainer)
                .WillCascadeOnDelete(false);
        }
    }
}

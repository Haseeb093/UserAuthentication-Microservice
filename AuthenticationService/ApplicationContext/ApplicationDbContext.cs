using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Services.ApplicationContext
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

            modelBuilder.Entity<Users>().Property(b => b.UpdatedBy).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Users>().Property(b => b.InsertedBy).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Users>().Property(b => b.InsertedDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Users>().Property(b => b.UpdatedDate).IsRequired().HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Departments>().Property(b => b.UpdatedBy).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Departments>().Property(b => b.InsertedBy).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Departments>().Property(b => b.InsertedDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Departments>().Property(b => b.UpdatedDate).IsRequired().HasDefaultValueSql("getdate()");


            modelBuilder.Entity<RoomTypes>().Property(b => b.UpdatedBy).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<RoomTypes>().Property(b => b.InsertedBy).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<RoomTypes>().Property(b => b.InsertedDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<RoomTypes>().Property(b => b.UpdatedDate).IsRequired().HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Blocks>().Property(b => b.UpdatedBy).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Blocks>().Property(b => b.InsertedBy).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Blocks>().Property(b => b.InsertedDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Blocks>().Property(b => b.UpdatedDate).IsRequired().HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Rooms>().Property(b => b.UpdatedBy).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Rooms>().Property(b => b.InsertedBy).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Rooms>().Property(b => b.InsertedDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Rooms>().Property(b => b.UpdatedDate).IsRequired().HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Stay>().Property(b => b.UpdatedBy).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Stay>().Property(b => b.InsertedBy).IsRequired().HasMaxLength(25);
            modelBuilder.Entity<Stay>().Property(b => b.InsertedDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Stay>().Property(b => b.UpdatedDate).IsRequired().HasDefaultValueSql("getdate()");
        }

        public DbSet<Cities> Cities { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<Genders> Genders { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<RoomTypes> RoomTypes { get; set; }
        public DbSet<Floors> Floors { get; set; }
        public DbSet<Blocks> Blocks { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<Stay> Stay { get; set; }
    }
}

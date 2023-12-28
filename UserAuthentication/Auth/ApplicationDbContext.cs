using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace UserAuthentication.Auth
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Roles>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

            modelBuilder.Entity<User>().Property(b => b.UpdatedDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<User>().Property(b => b.InsertedDate).HasDefaultValueSql("getdate()");

            base.OnModelCreating(modelBuilder);
        }
        //public DbSet<User> Users { get; set; }
        //public DbSet<Roles> Roles { get; set; }
    }
}

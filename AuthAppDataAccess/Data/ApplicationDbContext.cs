using AuthAppDataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AuthApp
{
    public class ApplicationDbContext : IdentityDbContext<User,Role,int,IdentityUserClaim<int>,UserRole,IdentityUserLogin<int>,RoleClaims,IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {
                
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Ignore<IdentityRoleClaim<int>>();
            builder.Ignore<IdentityUserRole<int>>();
            builder.Ignore<IdentityUserToken<int>>();   
            builder.Ignore<IdentityUserLogin<int>>();



            builder.Entity<User>(entity=>entity.ToTable(name:"Users"));
            builder.Entity<Role>(entity => { 
                entity.ToTable(name: "Roles");
            });
            builder.Entity<RoleClaims>(entity=> 
                {
                    entity.ToTable(name: "RoleClaims");
                    //entity.Ignore(e => e.ClaimType);
                    //entity.Ignore(e => e.ClaimValue);
                });
            builder.Entity<UserRole>(entity => {
                entity.ToTable(name: "UserRoles");
                entity.HasKey(e => new { e.RoleId, e.UserId });
                entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
                 entity.HasOne(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleId);
            });
            builder.Entity<IdentityUserClaim<int>>(entity => entity.ToTable(name: "UserClaims"));


    
        }
        public DbSet<RoleClaims> RoleClaims{ get; set; }
        public DbSet<UserRole> UserRoles{ get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }    
        

    }
}

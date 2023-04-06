using Intex_app.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.DataContext
{
    public class AuthDataContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AuthDataContext(DbContextOptions<AuthDataContext> options): base(options)
        {

        }

        public AuthDataContext()
        {

        }

       

        public static AuthDataContext Create()
        {
            return new AuthDataContext();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("IntexUser");
            modelBuilder.Entity<ApplicationRole>().ToTable("IntexRole");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("IntexUserRole");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("IntexUserClaim");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("IntexRoleClaim");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("IntexUserToken");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("IntexUserLogin");

        }
    }
}

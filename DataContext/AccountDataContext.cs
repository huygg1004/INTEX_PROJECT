using Intex_app.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.DataContext
{
    public class AccountDataContext : DbContext
    {
        public virtual DbSet<ApplicationUser> IntexUser { get; set; }

        public AccountDataContext(DbContextOptions<AccountDataContext> options) : base(options)
        {

        }

        public AccountDataContext()
        { }

        public static AccountDataContext Create()
        {
            return new AccountDataContext();
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<ApplicationUser>().ToTable("IntexUser");
        //    modelBuilder.Entity<ApplicationRole>().ToTable("IntexRole");
        //    modelBuilder.Entity<IdentityUserRole<string>>().ToTable("IntexUserRole");
        //    modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("IntexUserClaim");
        //    modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("IntexRoleClaim");
        //    modelBuilder.Entity<IdentityUserRole<string>>().ToTable("IntexUserToken");
        //    modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("IntexUserLogin");

        //}
    }
}

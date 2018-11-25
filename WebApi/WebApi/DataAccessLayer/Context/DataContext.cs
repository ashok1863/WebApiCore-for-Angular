using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataAccessLayer.Entities;
using WebApi.DataAccessLayer.Entities.Product;
using WebApi.Models;

namespace WebApi.DataAccessLayer.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {
            }


        public DbSet<AspnetUser> AspnetUsers { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<UserModel> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }


        public DbSet<Menu> Menus { get; set; }


        public DbSet<Permission> Permissions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>(p =>
                p.HasKey(c => new { c.UserId, c.RoleId })
                
                );
        }


    }
}

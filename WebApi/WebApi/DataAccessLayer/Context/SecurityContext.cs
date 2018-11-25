using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Helpers
{
    public class SecurityContext :  IdentityDbContext<ApplicationUser>
    {
        public SecurityContext(DbContextOptions<SecurityContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext:IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { 
        
        }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Address> Addresses { get; set; }


    }
}

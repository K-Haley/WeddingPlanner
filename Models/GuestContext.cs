using Microsoft.EntityFrameworkCore;
 
namespace WeddingPlanner.Models
{
    public class GuestContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public GuestContext(DbContextOptions<GuestContext> options) : base(options) { }

        public DbSet<User> Users {get;set;}
        public DbSet<Wedding> Weddings {get;set;}
        public DbSet<Guest> Guests {get;set;}
        
    }
}
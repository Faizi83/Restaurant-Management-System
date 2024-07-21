using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Resturant.Models;

namespace Resturant.Db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        public DbSet<bookedtables> bookings { get; set; }

        public DbSet<addfood> allfoods { get; set; }

        public DbSet<addtocart> cart { get; set; }

        public DbSet<register> Registers { get; set; }


    }
}

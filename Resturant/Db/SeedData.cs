using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Resturant.Models;
using System;
using System.Linq;

namespace Resturant.Db
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Registers.Any())
                {
                    return; // DB has been seeded
                }

                context.Registers.AddRange(
                    new register
                    {
                        Username = "Admin",
                        Email = "admin@example.com",
                        Password = "123",
                        Role = "Admin"
                    }
                );

                context.SaveChanges();
            }
        }
    }
}

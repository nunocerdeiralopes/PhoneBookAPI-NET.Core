using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PhoneBookAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;



namespace PhoneBookAPI.Data
{
    public class SeedData
    {
        private IHttpContextAccessor _httpContextAccessor;

        public SeedData(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Seeding database...");

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
                EnsureSeedData(context);
            }

            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }

        // Users hardcoded and passwords not hashed with salt for simplicity's sake
        private static List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Admin", LastName = "User", Username = "admin", Password = "admin", Role = Role.Admin },
            new User { Id = 2, FirstName = "Normal", LastName = "User", Username = "user", Password = "user", Role = Role.User }
        };

        private static void EnsureSeedData(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(_users);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("The example users already exist.");
            }
        }

    }
}

using System.Linq;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Models;

namespace StudyBuddy.Data
{
    public class DataBaseSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Ensure database is created
            context.Database.Migrate();

            // Check if the admin user already exists
            if (!context.Users.Any(u => u.Username == "admin"))
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword("adminadmin", 12);

                var adminUser = new User
                {
                    Username = "admin",
                    Email = "admin@gmail.com",
                    Password = hashedPassword,
                    Role = UserRole.Admin,
                    Avatar = "https://res.cloudinary.com/studybuddybackend/image/upload/v1743609233/p7jmtw4gcxflcpm4asag.jpg"
                };

                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }
    }
}

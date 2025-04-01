﻿using System.Linq;
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
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin", 12);

                var adminUser = new User
                {
                    Username = "admin",
                    Email = "admin@gmail.com",
                    Password = hashedPassword,
                    Role = UserRole.Admin
                };

                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }
    }
}

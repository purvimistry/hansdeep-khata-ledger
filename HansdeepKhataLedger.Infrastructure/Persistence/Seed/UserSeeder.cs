using HansdeepKhataLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Infrastructure.Persistence.Seed
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(AppDbContext dbContext)
        {
            if (await dbContext.Users.AnyAsync())
                return;

            if (!await dbContext.Roles.AnyAsync(r => r.Name == "Admin"))
            {
                dbContext.Roles.Add(new Role { Name = "Admin" });
            }

            if (!await dbContext.Roles.AnyAsync(r => r.Name == "User"))
            {
                dbContext.Roles.Add(new Role { Name = "User" });
            }

            await dbContext.SaveChangesAsync();
            

            var adminRole = await dbContext.Roles
                .FirstAsync(r => r.Name == "Admin");

            // read from environment variables
            var username = Environment.GetEnvironmentVariable("ADMIN_USERNAME");
            var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
            var email = Environment.GetEnvironmentVariable("ADMIN_EMAIL");

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("ADMIN_USERNAME or ADMIN_PASSWORD is not set in environment variables.");
            }

            var admin = new User
            {
                Username = username,
                Email = email ?? "admin@local.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                IsActive = true,
                RoleId = adminRole.Id,
                CreatedOn = DateTime.UtcNow,
            };
            dbContext.Users.Add(admin);
            await dbContext.SaveChangesAsync();
        }
    }
}

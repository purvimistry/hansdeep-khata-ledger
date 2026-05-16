using HansdeepKhataLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Infrastructure.Persistence.Seed
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(AppDbContext dbContext)
        {
            if (await dbContext.admins.AnyAsync())
                return;

            // read from environment variables
            var username = Environment.GetEnvironmentVariable("ADMIN_USERNAME");
            var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
            var email = Environment.GetEnvironmentVariable("ADMIN_EMAIL");

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("ADMIN_USERNAME or ADMIN_PASSWORD is not set in environment variables.");
            }

            var admin = new Admin
            {
                Username = username,
                Email = email ?? "admin@local.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                IsActive = true,
                CreatedOn = DateTime.Now
            };
            dbContext.admins.Add(admin);
            await dbContext.SaveChangesAsync();
        }
    }
}

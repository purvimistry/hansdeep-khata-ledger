using HansdeepKhataLedger.Application.Interfaces;
using HansdeepKhataLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;
        public AuthService(AppDbContext dbContext) 
        {
            _dbContext = dbContext; 
        }
        public async Task<bool> ValidateAdmin(string username, string password)
        {
            var admin = await _dbContext.Admins.FirstOrDefaultAsync(a => a.Username == username && a.IsActive);
            if (admin == null) { 
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash);
        }
    }
}

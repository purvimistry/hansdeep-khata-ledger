using HansdeepKhataLedger.Application.Interfaces.Repositories;
using HansdeepKhataLedger.Domain.Entities;
using HansdeepKhataLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _dbContext;
        public CustomerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext; 
        }
        public async Task AddAsync(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);  
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.Customers
                .AnyAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _dbContext.Customers
                .Include(x => x.Village)
                .Include(x => x.Area)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.FullName)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _dbContext.Customers
                 .Include(x => x.Village)
                 .Include(x => x.Area)
                 .FirstOrDefaultAsync(x =>
                     x.Id == id && !x.IsDeleted
                );

        }

        public void Update(Customer customer)
        {
            _dbContext.Customers.Update(customer);
        }
    }
}

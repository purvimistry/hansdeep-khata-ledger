using HansdeepKhataLedger.Application.Common;
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
                .AnyAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<PagedResult<Customer>> GetAllAsync(string? searchTerm, int? villageId, int? areaId, int page, int pageSize)
        {
            var query = _dbContext.Customers
                .Include(c => c.Village)
                .Include(c => c.Area)
                .Where(c => !c.IsDeleted && c.IsActive)
                .AsQueryable();
            if(!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c =>
                    c.FullName.Contains(searchTerm) ||
                    c.MobileNumber.Contains(searchTerm.Trim()));
            }
            if(villageId.HasValue)
            {
                query = query.Where(c => c.VillageId == villageId);
            }
            if (areaId.HasValue)
            {
                query = query.Where(c => c.AreaId == areaId); 
            }
            var totalRecords = await query.CountAsync();
            var customers = await query
                .OrderBy(c => c.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Customer>
            {
                Items = customers,
                CurrentPage = page,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _dbContext.Customers
                 .Include(c => c.Village)
                 .Include(c => c.Area)
                 .FirstOrDefaultAsync(c =>
                     c.Id == id && !c.IsDeleted
                );
        }

        public void Update(Customer customer)
        {
            _dbContext.Customers.Update(customer);
        }
        public async Task<bool> MobileNumberExistsAsync(string mobileNumber, int? excludeCustomerId = null)
        {
            return await _dbContext.Customers.AnyAsync(c =>
                c.MobileNumber == mobileNumber &&
                (!excludeCustomerId.HasValue || c.Id != excludeCustomerId.Value));
        }
    }
}

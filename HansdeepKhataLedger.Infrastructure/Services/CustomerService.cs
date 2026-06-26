using HansdeepKhataLedger.Application.Interfaces.Repositories;
using HansdeepKhataLedger.Application.Interfaces.Services;
using HansdeepKhataLedger.Domain.Entities;
using HansdeepKhataLedger.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _dbContext;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(AppDbContext dbContext, ICustomerRepository customerRepository)
        {
            _dbContext = dbContext;
            _customerRepository = customerRepository;
        }

        public async Task AddCustomerAsync(Customer customer,  int userId)
        {
            customer.CreatedByUserId = userId;
            customer.CreatedAt = DateTime.UtcNow;
             await _customerRepository.AddAsync(customer);    
            await _dbContext.SaveChangesAsync();    
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task UpdateCustomerAsync(Customer customer,int userId)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(customer.Id);

            if (existingCustomer == null)
                throw new Exception("Customer not found.");

            existingCustomer.FullName = customer.FullName;
            existingCustomer.MobileNumber = customer.MobileNumber;
            existingCustomer.AlternateMobileNumber = customer.AlternateMobileNumber;
            existingCustomer.VillageId = customer.VillageId;
            existingCustomer.AreaId = customer.AreaId;
            existingCustomer.Notes = customer.Notes;
            existingCustomer.AdvanceBalance = customer.AdvanceBalance;
            existingCustomer.IsActive = customer.IsActive;

            existingCustomer.UpdatedAt = DateTime.UtcNow;
            existingCustomer.UpdatedByUserId = userId;
            await _dbContext.SaveChangesAsync();
        }
    }
}

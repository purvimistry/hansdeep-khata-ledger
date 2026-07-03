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
        private readonly IVillageRepository _villageRepository;
        private readonly IAreaRepository _areaRepository;

        public CustomerService(AppDbContext dbContext, ICustomerRepository customerRepository, IVillageRepository villageRepository, IAreaRepository areaRepository )
        {
            _dbContext = dbContext;
            _customerRepository = customerRepository;
            _villageRepository = villageRepository;
            _areaRepository = areaRepository;
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
                throw new Exception("Customer not found");

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

        public async Task<List<Village>> GetVillagesAsync()
        {
            return await _villageRepository.GetAllAsync();
        }
        public async Task<Village> AddVillageAsync(string name)
        {
            
            if (string.IsNullOrWhiteSpace(name.Trim()))
                throw new Exception("Village name is required");

            var existingVillage =await _villageRepository.GetByNameAsync(name);
            if (existingVillage != null)
                throw new Exception("Village already exists");

            var village = new Village
            {
                Name = name
            };
            await _villageRepository.AddAsync(village);
            await _dbContext.SaveChangesAsync();

            return village;
        }
        public async Task<Area> AddAreaAsync(string name, int villageId)
        {
            
            if (string.IsNullOrWhiteSpace(name.Trim()))
                throw new Exception("Area name is required");


            var village = await _villageRepository.GetByIdAsync(villageId);
            if (village == null)
                throw new Exception("Village not found");


            var existingArea = await _areaRepository.GetByNameAsync(name, villageId);
            if (existingArea != null)
                return existingArea;

            var area = new Area
            {
                VillageId = villageId,
                Name = name
            };
            await _areaRepository.AddAsync(area);
            await _dbContext.SaveChangesAsync();

            return area;
        }
        public async Task<List<Area>> GetAreasByVillageAsync(int villageId)
        {
            return await _areaRepository.GetByVillageIdAsync(villageId);
        }
       

    }
}

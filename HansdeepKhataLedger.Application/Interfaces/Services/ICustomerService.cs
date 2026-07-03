using HansdeepKhataLedger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task AddCustomerAsync(Customer customer, int userId);
        Task UpdateCustomerAsync(Customer customer, int userId);
        Task<List<Village>> GetVillagesAsync();
        Task<Village> AddVillageAsync(string name);
        Task<List<Area>> GetAreasByVillageAsync(int villageId);
        Task<Area> AddAreaAsync(string name, int villageId);

    }
}

using HansdeepKhataLedger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Application.Interfaces.Repositories
{
    public interface IAreaRepository
    {
        Task<List<Area>> GetAllAsync();
        Task<List<Area>> GetByVillageIdAsync(int villageId);
        Task<Area?> GetByIdAsync(int id);
        Task AddAsync(Area area);
    }
}

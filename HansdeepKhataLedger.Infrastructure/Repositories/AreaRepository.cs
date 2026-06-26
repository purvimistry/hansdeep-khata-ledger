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
    public class AreaRepository : IAreaRepository
    {
        private readonly AppDbContext _dbContext;
        public AreaRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Area area)
        {
            await _dbContext.Areas.AddAsync(area);
        }

        public async Task<List<Area>> GetAllAsync()
        {
            return await _dbContext.Areas
                .Include(x => x.Village)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Area?> GetByIdAsync(int id)
        {
            return await _dbContext.Areas
                .Include(x => x.Village)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Area>> GetByVillageIdAsync(int villageId)
        {
            return await _dbContext.Areas
                .Where(x => x.VillageId == villageId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}

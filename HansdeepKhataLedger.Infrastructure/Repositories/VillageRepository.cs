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
    public class VillageRepository : IVillageRepository
    {
        private readonly AppDbContext _dbContext;
        public VillageRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Village village)
        {
            await _dbContext.Villages.AddAsync(village);   
        }

        public async Task<List<Village>> GetAllAsync()
        {
            return await _dbContext.Villages
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Village?> GetByIdAsync(int id)
        {
            return await _dbContext.Villages
                .FirstOrDefaultAsync(x => x.Id == id); 

        }
        public async Task<Village?> GetByNameAsync(string name)
        {
            return await _dbContext.Villages.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }
    }
}

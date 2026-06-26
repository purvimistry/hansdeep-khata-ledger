using HansdeepKhataLedger.Application.Interfaces.Repositories;
using HansdeepKhataLedger.Application.Interfaces.Services;
using HansdeepKhataLedger.Infrastructure.Persistence;
using HansdeepKhataLedger.Infrastructure.Repositories;
using HansdeepKhataLedger.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(
                        typeof(AppDbContext).Assembly.GetName().Name)
                    ));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IVillageRepository, VillageRepository>();
            services.AddScoped<IAreaRepository, AreaRepository>();

            services.AddScoped<ICustomerService, CustomerService>();
            return services;
        }
    }
}

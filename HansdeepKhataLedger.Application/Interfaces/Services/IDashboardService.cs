using HansdeepKhataLedger.Application.Common.HansdeepKhataLedger.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DashboardData> GetDashboardDataAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Application.Interfaces
{
    public interface IAuthService
    {
        Task<bool> ValidateAdmin(string username, string password);
    }
}

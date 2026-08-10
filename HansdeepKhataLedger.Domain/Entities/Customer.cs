using HansdeepKhataLedger.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string? MobileNumber {  get; set; }
        public string? AlternateMobileNumber { get; set; }
        public int VillageId { get; set; }
        public Village Village { get; set; } = null;
        public int? AreaId { get; set;}
        public Area? Area { get; set; }
        public string? Notes { get; set; }  
        public decimal AdvanceBalance {  get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<LedgerEntry> LedgerEntries { get; set; } = new List<LedgerEntry>();

    }
}

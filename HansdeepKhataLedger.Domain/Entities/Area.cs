using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Domain.Entities
{
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int VillageId {  get; set; }
        public Village Village { get; set; } = null;
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Domain.Entities
{
    public class Village
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public ICollection<Area> Areas { get; set; } = new List<Area>();
    }
}

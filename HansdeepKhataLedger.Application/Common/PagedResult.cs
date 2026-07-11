using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Application.Common
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int CurrentPage {  get; set; }
        public int PageSize {  get; set; }
        public int TotalRecords {  get; set; }
        public int TotalPages =>
            (int)Math.Ceiling((double)TotalRecords / PageSize);
    }
}

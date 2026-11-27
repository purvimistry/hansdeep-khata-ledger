using HansdeepKhataLedger.Domain.Entities.Common.Reports;

namespace HansdeepKhataLedger.Web.Models.Reports
{
    public class ReportsIndexViewModel
    {
        public string? SearchTerm { get; set; }

        public int TotalCustomers { get; set; }

        public int CustomersWithDues { get; set; }

        public decimal TotalCredit { get; set; }

        public decimal TotalCollection { get; set; }

        public decimal TotalOutstanding { get; set; }

        public List<CustomerOutstandingReport> CustomerOutstandingReports { get; set; } = new();
    }
}

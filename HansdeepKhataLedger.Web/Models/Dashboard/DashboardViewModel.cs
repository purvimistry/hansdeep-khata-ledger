namespace HansdeepKhataLedger.Web.Models.Dashboard
{
    public class DashboardViewModel
    {
        public int TotalCustomers {  get; set; }
        public decimal TotalPendingAmount {  get; set; }
        public decimal TodaysCredit {  get; set; }
        public decimal TodaysCollection {  get; set; }
        public int TodaysCreditEntries { get; set; }
        public List<RecentTransactionViewModel> RecentLedgerEntries { get; set; } = new();
        public List<RecentTransactionViewModel> RecentPayments { get; set; } = new();

    }
}

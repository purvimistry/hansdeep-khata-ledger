namespace HansdeepKhataLedger.Web.Models.Dashboard
{
    public class RecentTransactionViewModel
    {
        public string CustomerName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime EntryDate { get; set; }
        public string? Description { get; set; }
    }
}

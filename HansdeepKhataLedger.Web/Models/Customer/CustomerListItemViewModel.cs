namespace HansdeepKhataLedger.Web.Models.Customer
{
    public class CustomerListItemViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set;} = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string VillageName {  get; set; } = string.Empty;
        public string? AreaName { get; set; }
        public decimal PendingBalance { get; set; } = 0;
        public DateTime? LastTransactionDate { get; set; }
        public bool IsActive { get; set; }
    }
}

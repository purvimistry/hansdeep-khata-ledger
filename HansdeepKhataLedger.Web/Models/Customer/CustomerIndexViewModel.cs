namespace HansdeepKhataLedger.Web.Models.Customer
{
    public class CustomerIndexViewModel
    {
        public List<CustomerListItemViewModel> Customers { get; set; } = new();
        public string? SearchTerm { get; set; }
        public int? VillageId { get; set; }
        public int? AreaId { get; set; }
        public bool? IsActive { get; set; }
        public string SortBy { get; set; } = "Name";
        public bool SortDescending { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
    }
}

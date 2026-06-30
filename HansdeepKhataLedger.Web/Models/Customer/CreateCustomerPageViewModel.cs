using Microsoft.AspNetCore.Mvc.Rendering;

namespace HansdeepKhataLedger.Web.Models.Customer
{
    public class CreateCustomerPageViewModel
    {
        public CreateCustomerViewModel Customer { get; set; } = new();
        public List<SelectListItem> Villages { get; set; } = new();
        public List<SelectListItem> Areas { get; set; } = new();
    }
}

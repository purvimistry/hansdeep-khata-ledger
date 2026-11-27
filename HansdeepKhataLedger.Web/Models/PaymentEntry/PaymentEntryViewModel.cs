using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HansdeepKhataLedger.Web.Models.PaymentEntry
{
    public class PaymentEntryViewModel
    {
        [Required(ErrorMessage = "Please select a customer")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please enter an amount")]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Please select an entry date")]
        public DateTime EntryDate { get; set; } = DateTime.Today;

        [StringLength(250,
            ErrorMessage = "Description cannot exceed 250 characters")]
        public string? Description { get; set; }

        public List<SelectListItem> Customers { get; set; } = new();
    }
}

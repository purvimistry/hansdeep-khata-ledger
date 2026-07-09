using System.ComponentModel.DataAnnotations;

namespace HansdeepKhataLedger.Web.Models.Customer
{
    public class CustomerFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(100, ErrorMessage = "Customer name cannot exceed 100 characters.")]
        public string FullName {  get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^[6-9]\d{9}$",
           ErrorMessage = "Please enter a valid 10-digit mobile number.")]
        public string MobileNumber { get; set; } = string.Empty;

        [RegularExpression(@"^[6-9]\d{9}$",
            ErrorMessage = "Please enter a valid 10-digit mobile number.")]
        public string? AlternateMobileNumber {  get; set; }

        [Required(ErrorMessage = "Please select a village.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a village.")]
        public int VillageId {  get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select an area.")]
        public int? AreaId {  get; set; }

        [StringLength(100, ErrorMessage = "Notes cannot exceed 100 characters.")]
        public string? Notes {  get; set; }

        [Range(0, 999999999.99,
           ErrorMessage = "Advance balance cannot be negative.")]
        public decimal? AdvanceBalance { get; set; }
    }
}

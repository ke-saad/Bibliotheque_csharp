using System.ComponentModel.DataAnnotations;

namespace MvcLibrary.Models
{
    public class ManageAccountViewModel
    {
        [Required]
        public string FullName { get; set; }

        // Add other properties as needed

        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}

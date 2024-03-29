using System.ComponentModel.DataAnnotations;

namespace MvcLibrary.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace MvcLibrary.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FullName { get; set; }

        // If these can be null, mark them as nullable
        public string? Semester { get; set; }
        public string? Department { get; set; }
    }

}

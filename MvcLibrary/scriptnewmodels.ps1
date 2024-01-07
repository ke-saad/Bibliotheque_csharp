$modelsFolderPath = "C:\Users\DELL\Desktop\4IIR\.NET\Models"

# Create Models folder if it doesn't exist
if (-not (Test-Path $modelsFolderPath -PathType Container)) {
    New-Item -ItemType Directory -Path $modelsFolderPath | Out-Null
}

# LoginViewModel.cs
$loginViewModelContent = @'
using System.ComponentModel.DataAnnotations;

namespace MvcLibrary.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
'@
$loginViewModelPath = Join-Path $modelsFolderPath "LoginViewModel.cs"
$loginViewModelContent | Out-File -FilePath $loginViewModelPath

# RegisterViewModel.cs
$registerViewModelContent = @'
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

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FullName { get; set; }

        // Add other properties as needed (e.g., Semester, Department)
        public string Semester { get; set; }
        public string Department { get; set; }
    }
}
'@
$registerViewModelPath = Join-Path $modelsFolderPath "RegisterViewModel.cs"
$registerViewModelContent | Out-File -FilePath $registerViewModelPath

# ForgotPasswordViewModel.cs
$forgotPasswordViewModelContent = @'
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
'@
$forgotPasswordViewModelPath = Join-Path $modelsFolderPath "ForgotPasswordViewModel.cs"
$forgotPasswordViewModelContent | Out-File -FilePath $forgotPasswordViewModelPath

# ManageAccountViewModel.cs
$manageAccountViewModelContent = @'
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
'@
$manageAccountViewModelPath = Join-Path $modelsFolderPath "ManageAccountViewModel.cs"
$manageAccountViewModelContent | Out-File -FilePath $manageAccountViewModelPath

Write-Host "Model files created successfully."

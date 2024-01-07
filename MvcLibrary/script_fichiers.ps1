# Set the root directory of your project
$projectRoot = "C:\Users\DELL\Desktop\4IIR\.NET\MvcLibrary"

# Define the namespace and class content
$namespace = "MvcLibrary.Models"
$classContent = @"
using System;
using System.ComponentModel.DataAnnotations;

namespace $namespace
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
"@

# Define the file path
$filePath = Join-Path $projectRoot "Models\ResetPasswordViewModel.cs"

# Create the directory if it doesn't exist
$directory = [System.IO.Path]::GetDirectoryName($filePath)
if (-not (Test-Path -Path $directory)) {
    New-Item -Path $directory -ItemType Directory -Force
}

# Write the content to the file
$classContent | Out-File -FilePath $filePath -Force

Write-Host "ResetPasswordViewModel.cs created successfully at: $filePath"

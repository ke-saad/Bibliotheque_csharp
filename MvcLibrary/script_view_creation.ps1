# Specify the root directory of your ASP.NET Core MVC project
$projectRoot = "C:\Users\DELL\Desktop\4IIR\.NET\MvcLibrary"

# Specify the directory for Views
$viewsDirectory = Join-Path $projectRoot "Views"

# Specify the directory for Account views
$accountViewsDirectory = Join-Path $viewsDirectory "Account"

# Content for Manage.cshtml
$manageContent = @"
@model MvcLibrary.Models.ManageAccountViewModel

<h2>Manage Your Account</h2>

<form asp-action="Manage" method="post">
    <!-- Your form fields here -->
    <label asp-for="FullName"></label>
    <input asp-for="FullName" />

    <label asp-for="OldPassword"></label>
    <input asp-for="OldPassword" />

    <label asp-for="NewPassword"></label>
    <input asp-for="NewPassword" />

    <label asp-for="ConfirmNewPassword"></label>
    <input asp-for="ConfirmNewPassword" />

    <button type="submit">Update</button>
</form>
"@

# Content for ForgotPassword.cshtml
$forgotPasswordContent = @"
@model MvcLibrary.Models.ForgotPasswordViewModel

<h2>Forgot Password</h2>

<form asp-action="ForgotPassword" method="post">
    <!-- Your form fields here -->
    <label asp-for="Email"></label>
    <input asp-for="Email" />

    <button type="submit">Send Reset Email</button>
</form>
"@

# Content for ResetPassword.cshtml
$resetPasswordContent = @"
@model MvcLibrary.Models.ResetPasswordViewModel

<h2>Reset Password</h2>

<form asp-action="ResetPassword" method="post">
    <!-- Your form fields here -->
    <label asp-for="Token"></label>
    <input asp-for="Token" />

    <label asp-for="NewPassword"></label>
    <input asp-for="NewPassword" />

    <label asp-for="ConfirmPassword"></label>
    <input asp-for="ConfirmPassword" />

    <button type="submit">Reset Password</button>
</form>
"@

# Create Manage.cshtml
$manageFilePath = Join-Path $accountViewsDirectory "Manage.cshtml"
$manageContent | Out-File -FilePath $manageFilePath -Force

# Create ForgotPassword.cshtml
$forgotPasswordFilePath = Join-Path $accountViewsDirectory "ForgotPassword.cshtml"
$forgotPasswordContent | Out-File -FilePath $forgotPasswordFilePath -Force

# Create ResetPassword.cshtml
$resetPasswordFilePath = Join-Path $accountViewsDirectory "ResetPassword.cshtml"
$resetPasswordContent | Out-File -FilePath $resetPasswordFilePath -Force

Write-Host "Files created successfully:"
Write-Host " - $manageFilePath"
Write-Host " - $forgotPasswordFilePath"
Write-Host " - $resetPasswordFilePath"

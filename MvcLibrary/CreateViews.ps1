# PowerShell script to create views for Error and Success messages

# Define paths for the new files
$viewsPath = "C:\Users\DELL\Desktop\4IIR\.NET\MvcLibrary\Views\Home"
$errorViewPath = Join-Path -Path $viewsPath -ChildPath "Error.cshtml"
$successViewPath = Join-Path -Path $viewsPath -ChildPath "Success.cshtml"

# Define the content for each view
$errorContent = @"
@{
    ViewData[""Title""] = "Error";
}

<h2>Error</h2>
@if (TempData["ErrorMessage"] is string errorMessage)
{
    <div class="alert alert-danger">@errorMessage</div>
}
<a href="@Url.Action("Index", "Home")" class="btn btn-primary">Go Back to Home</a>
"@

$successContent = @"
@{
    ViewData[""Title""] = "Success";
}

<h2>Success</h2>
@if (TempData["SuccessMessage"] is string successMessage)
{
    <div class="alert alert-success">@successMessage</div>
}
<a href="@Url.Action("Index", "Home")" class="btn btn-primary">Go Back to Home</a>
"@

# Check if the Views\Home directory exists, if not create it
if (-not (Test-Path -Path $viewsPath -PathType Container)) {
    New-Item -Path $viewsPath -ItemType Directory
}

# Create the Error view
if (-not (Test-Path -Path $errorViewPath)) {
    Set-Content -Path $errorViewPath -Value $errorContent
}

# Create the Success view
if (-not (Test-Path -Path $successViewPath)) {
    Set-Content -Path $successViewPath -Value $successContent
}

# Output the result
Write-Host "Views created successfully."

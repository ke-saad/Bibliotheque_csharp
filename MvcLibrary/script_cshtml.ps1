# Define the root path of the Views directory
$viewsRootPath = "C:\Users\DELL\Desktop\4IIR\.NET\MvcLibrary\Views"

# Get all cshtml files recursively
$cshtmlFiles = Get-ChildItem -Path $viewsRootPath -Filter *.cshtml -Recurse

# Loop through each file and display its contents
foreach ($file in $cshtmlFiles) {
    Write-Host "Contents of file: $($file.FullName)" -ForegroundColor Cyan
    Get-Content -Path $file.FullName
    Write-Host "End of file: $($file.FullName)" -ForegroundColor Cyan
    Write-Host "`n" # Add extra newline for readability
}

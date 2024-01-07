# PowerShell script to display the contents of specific files in an ASP.NET Core project

$projectRoot = "C:\Users\DELL\Desktop\4IIR\.NET\Library\MvcLibrary"

# Define specific file paths
$specificFiles = @(
    "Startup.cs",
    "Program.cs",
    "Data\ApplicationDbContext.cs"
)

# Combine all specific file paths with their root path
$specificFilePaths = $specificFiles | ForEach-Object { Join-Path $projectRoot $_ }

# Add all .cs files in Models and Controllers directories
$modelFiles = Get-ChildItem -Path "$projectRoot\Models" -Filter *.cs -Recurse
$controllerFiles = Get-ChildItem -Path "$projectRoot\Controllers" -Filter *.cs -Recurse

# Get all files in the Views directory recursively, excluding directories
$viewFiles = Get-ChildItem -Path "$projectRoot\Views" -Recurse | Where-Object { -not $_.PSIsContainer }

# Combine all file paths
$allFiles = $specificFilePaths + $modelFiles.FullName + $controllerFiles.FullName + $viewFiles.FullName

# Display the contents of each file
foreach ($filePath in $allFiles) {
    if (Test-Path $filePath) {
        Write-Host "Contents of $(Split-Path $filePath -Leaf):`n" -ForegroundColor Green
        Get-Content $filePath
        Write-Host "`n`n" # Add extra new lines for readability
    } else {
        Write-Host "File not found: $(Split-Path $filePath -Leaf)" -ForegroundColor Red
    }
}

# Set the path to the Views directory
$viewsPath = ".\Views"

# Get all .cshtml files recursively
$cshtmlFiles = Get-ChildItem -Path $viewsPath -Filter *.cshtml -Recurse

# Display content of each .cshtml file
foreach ($file in $cshtmlFiles) {
    $filePath = $file.FullName
    $content = Get-Content -Path $filePath -Raw
    Write-Host "File: $filePath"
    Write-Host "----------------------------------------"
    Write-Host $content
    Write-Host "----------------------------------------`n"
}

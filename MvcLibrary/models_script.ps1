# Set the path to the Models directory
$modelsPath = ".\Models"

# Get all .cs files recursively
$csFiles = Get-ChildItem -Path $modelsPath -Filter *.cs -Recurse

# Display content of each .cs file
foreach ($file in $csFiles) {
    $filePath = $file.FullName
    $content = Get-Content -Path $filePath -Raw
    Write-Host "File: $filePath"
    Write-Host "----------------------------------------"
    Write-Host $content
    Write-Host "----------------------------------------`n"
}

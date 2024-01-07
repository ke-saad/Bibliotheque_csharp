# Set the path to the Controllers directory
$controllersPath = ".\Controllers"

# Get all .cs files recursively
$csFiles = Get-ChildItem -Path $controllersPath -Filter *.cs -Recurse

# Display content of each .cs file
foreach ($file in $csFiles) {
    $filePath = $file.FullName
    $content = Get-Content -Path $filePath -Raw
    Write-Host "File: $filePath"
    Write-Host "----------------------------------------"
    Write-Host $content
    Write-Host "----------------------------------------`n"
}

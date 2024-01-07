# Get the list of installed packages for the project
$packages = dotnet list package --include-transitive | Out-String | ConvertFrom-Csv -Delimiter ' '

# Filter packages related to itext7.bouncy-castle-adapter and itext7.bouncy-castle-fips-adapter
$itextAdapterPackages = $packages | Where-Object { $_.'Package Id' -eq 'itext7.bouncy-castle-adapter' -or $_.'Package Id' -eq 'itext7.bouncy-castle-fips-adapter' }

# Count the number of occurrences for each adapter
$adapterCount = $itextAdapterPackages | Group-Object 'Package Id' | Select-Object Count, Name

# Check if both adapters are present and print the result
if ($adapterCount.Count -eq 2) {
    Write-Host "Both itext7.bouncy-castle-adapter and itext7.bouncy-castle-fips-adapter are present in the project. Remove one of them."
} elseif ($adapterCount.Count -eq 0) {
    Write-Host "Neither itext7.bouncy-castle-adapter nor itext7.bouncy-castle-fips-adapter is present in the project. Add one of them."
} else {
    Write-Host "Project has $($adapterCount.Name) adapter."
}

function Show-Tree {
    param (
        [string]$Path = ".",
        [string]$Indent = ""
    )

    Get-ChildItem -Path $Path | ForEach-Object {
        if ($_.PSIsContainer) {
            Write-Host "$Indent$($_.Name)"
            Show-Tree -Path $_.FullName -Indent ($Indent + "    ")
        } else {
            $size = 0
            if ($_ | Test-Path -PathType Leaf) {
                $size = Get-Item $_.FullName | Measure-Object -Property Length -Sum
            }
            Write-Host "$Indent$($_.Name) - Size: $($size.Sum) bytes"
        }
    }
}

Show-Tree -Path "C:\Users\DELL\Desktop\4IIR\.NET\MvcLibrary"

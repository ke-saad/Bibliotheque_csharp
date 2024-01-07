$url = "https://nodejs.org/dist/latest/win-x64/node.exe"
$output = "$env:TEMP\node.exe"
Invoke-WebRequest -Uri $url -OutFile $output
Start-Process -FilePath $output -Wait

# Définition du chemin de base pour le projet
$projectPath = "C:\Users\DELL\Desktop\4IIR\.NET\Bibliotheque"

# Création des dossiers de structure de projet
$folders = @(
    "\Models",
    "\Data",
    "\Controllers",
    "\Views",
    "\Views\Books",
    "\Views\Members",
    "\Views\Loans",
    "\Services",
    "\wwwroot",
    "\wwwroot\css",
    "\wwwroot\js",
    "\wwwroot\lib"
)

foreach ($folder in $folders) {
    $dir = $projectPath + $folder
    if (-not (Test-Path $dir)) {
        New-Item -ItemType Directory -Force -Path $dir
    }
}

# Création des fichiers modèles
$modelFiles = @("Book.cs", "Member.cs", "Loan.cs")

foreach ($file in $modelFiles) {
    $filePath = $projectPath + "\Models\" + $file
    if (-not (Test-Path $filePath)) {
        New-Item -ItemType File -Force -Path $filePath
    }
}

# Création des fichiers de données
$dataFiles = @("ApplicationDbContext.cs")

foreach ($file in $dataFiles) {
    $filePath = $projectPath + "\Data\" + $file
    if (-not (Test-Path $filePath)) {
        New-Item -ItemType File -Force -Path $filePath
    }
}

# Création des fichiers contrôleurs
$controllerFiles = @("BooksController.cs", "MembersController.cs", "LoansController.cs")

foreach ($file in $controllerFiles) {
    $filePath = $projectPath + "\Controllers\" + $file
    if (-not (Test-Path $filePath)) {
        New-Item -ItemType File -Force -Path $filePath
    }
}

# Création des fichiers de vues (seulement des index pour l'exemple)
$viewFiles = @("\Books\Index.cshtml", "\Members\Index.cshtml", "\Loans\Index.cshtml")

foreach ($file in $viewFiles) {
    $filePath = $projectPath + "\Views" + $file
    if (-not (Test-Path $filePath)) {
        New-Item -ItemType File -Force -Path $filePath
    }
}

# Création d'autres fichiers essentiels
$rootFiles = @("Startup.cs", "Program.cs", "appsettings.json", "appsettings.Development.json")

foreach ($file in $rootFiles) {
    $filePath = $projectPath + "\" + $file
    if (-not (Test-Path $filePath)) {
        New-Item -ItemType File -Force -Path $filePath
    }
}

Write-Host "La structure de projet a été créée avec succès."

# MyCalc Runner Script
# Builds and runs the MyCalc CLI application in Release mode for optimal performance

Write-Host "Building MyCalc (Release)..." -ForegroundColor Green
dotnet build --configuration Release

if ($LASTEXITCODE -eq 0) {
    Write-Host "Starting MyCalc CLI..." -ForegroundColor Green
    Write-Host ""
    dotnet run --project MyCalcCli --configuration Release
} else {
    Write-Host "Build failed. Please fix errors and try again." -ForegroundColor Red
}

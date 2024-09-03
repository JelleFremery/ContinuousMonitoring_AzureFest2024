[CmdletBinding()]
param (
  [Parameter()]
  [Alias('RGN')]
  [string] $ResourceGroupName = 'Betabit-AzureFest',

  [Parameter()]
  [Alias('ACRN')]
  [string] $AzureContainerRegistryName = 'betabitazurefestacr'
)

$version = Get-Date -Format 'yyyyMMdd-HHmmss'

Push-Location

try 
{
  Set-Location $PSScriptRoot/DemoApp/DemoApp.AppHost

  az acr login --name $AzureContainerRegistryName

  # Create the DemoApp images.
  aspirate build --disable-secrets --disable-state --container-registry "${AzureContainerRegistryName}.azurecr.io" `
        --non-interactive --project-path . --container-image-tag latest --container-image-tag $version `
        --container-builder docker

  # Run the images.
  az containerapp update --name apiservice --resource-group $ResourceGroupName --image "${AzureContainerRegistryName}.azurecr.io/apiservice:${version} "
  az containerapp update --name webfrontend --resource-group $ResourceGroupName --image "${AzureContainerRegistryName}.azurecr.io/webfrontend:${version}"

  Set-Location $PSScriptRoot/AlertToTeams

  # Create the image.
  az acr build --registry $AzureContainerRegistryName --image alerttoteams:$version --file .\AlertToTeams\Dockerfile .
  # Run the image.
  az containerapp update --name allerttoteams --resource-group $ResourceGroupName --image "${AzureContainerRegistryName}.azurecr.io/alerttoteams:${version}"
}
finally 
{
  Pop-Location
}
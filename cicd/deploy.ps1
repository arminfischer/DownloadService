[CmdletBinding()]
param (
    [Parameter(Mandatory = $True)] $ResourceGroup,
    $Location = "westeurope",
    [Parameter(Mandatory = $True)] $StorageAccountName,
    $ContainerNameFiles = "download-files",
    $TableNameReleases = "Releases",
    $TableNameCodes = "Codes",
    [Parameter(Mandatory = $True)] $DownloadServiceAppName,
    $SourceFolder = "$PSScriptRoot\..\src",
    $Configuration = "Release"
)

function CreateResourceGroupIfNotExists {
    if ((az group exists --name $ResourceGroup | ConvertFrom-Json) -ne $True) {
        az group create --name $ResourceGroup --location "$Location"
    }
}

function CreateStorageAccountIfNotExists {
    az storage account create --name $StorageAccountName --resource-group $ResourceGroup --https-only true --kind StorageV2
}

function CreateContainerIfNotExists {
    param($Name)

    if ((az storage container exists --name $Name --account-name $StorageAccountName | ConvertFrom-Json).exists -ne $True) {
        az storage container create --name $Name --account-name $StorageAccountName
    }
    else {
        Write-Host -ForegroundColor Green "Container ""$Name"" already exists"
    }
}

function CreateTableIfNotExists {
    param($Name)

    if ((az storage table exists --name $Name --account-name $StorageAccountName | ConvertFrom-Json).exists -ne $True) {
        az storage table create --name $Name --account-name $StorageAccountName
    }
    else {
        Write-Host -ForegroundColor Green "Table ""$Name"" already exists"
    }
}

function CreateFunctionAppIfNotExists {
    param($Name)

    az functionapp create --name $Name --resource-group $ResourceGroup --storage-account $StorageAccountName --functions-version 3 --runtime dotnet --consumption-plan-location $Location
}

function BuildAndPublishFunctionApp {
    param($ZipLocation)

    # Restore, build and test (implicitly restores and builds the dependend Function project)
    dotnet restore "$SourceFolder\DownloadService.Test"
    dotnet build "$SourceFolder\DownloadService.Test" --no-restore --configuration $Configuration
    dotnet test "$SourceFolder\DownloadService.Test" --no-build --configuration $Configuration

    if ($lastexitcode -eq 1) {
        exit
    }
    
    # Publish and zip
    dotnet publish "$SourceFolder\DownloadService" --no-build --configuration $Configuration -o "publish_tmp"
    Compress-Archive -Path "publish_tmp\*" -DestinationPath "$ZipLocation"

    # Deploy to Azure Function App
    az functionapp deployment source config-zip --resource-group $ResourceGroup --name $DownloadServiceAppName --src "$ZipLocation"

    # Clean up
    Remove-Item "publish_tmp" -Recurse -Force
}

function DeployFunctionApp {
    param($Name, $ZipLocation, [Switch]$DeleteZip)

    # Deploy to Azure Function App
    az functionapp deployment source config-zip --resource-group $ResourceGroup --name $Name --src "$ZipLocation"

    # Clean up
    if ($DeleteZip.IsPresent) {
        Remove-Item "$ZipLocation" -Force
    }    
}

function ConfigureSettingsAndServices {
    az functionapp config appsettings set --name $DownloadServiceAppName --resource-group $ResourceGroup --settings `
        "TableNameCodes=$TableNameCodes" `
        "TableNameReleases=$TableNameReleases" `
        "ContainerNameFiles=$ContainerNameFiles"
}

CreateResourceGroupIfNotExists
CreateStorageAccountIfNotExists
CreateContainerIfNotExists -Name $ContainerNameFiles
CreateTableIfNotExists -Name $TableNameReleases
CreateTableIfNotExists -Name $TableNameCodes
CreateFunctionAppIfNotExists -Name $DownloadServiceAppName
BuildAndPublishFunctionApp -ZipLocation "publish.zip"
DeployFunctionApp -Name $DownloadServiceAppName -ZipLocation "publish.zip" -DeleteZip
ConfigureSettingsAndServices
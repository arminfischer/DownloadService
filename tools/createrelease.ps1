[CmdletBinding()]
param (
    [Parameter(Mandatory = $True)] $ResourceGroup,
    [Parameter(Mandatory = $True)] $StorageAccountName,
    $ContainerNameFiles = "download-files",
    $TableNameReleases = "Releases",
    $TableNameCodes = "Codes",
    [Parameter(Mandatory = $True)] $ReleaseName,
    $MaxAllowedDownloads = 0,
    [Parameter(Mandatory = $True)] $Path,
    [Parameter(Mandatory = $True)] $NumberOfCodes,
    [Parameter(Mandatory = $True)] $CodePrefix,
    $User = "default"
)

function Get-RandomString {
    param($Length = 6)

    -join ((65..90) + (97..122) | Get-Random -Count $Length | ForEach-Object {[char]$_})
}

$fileName = "$ReleaseName/$((Get-Item $Path).Name)"

# Create release
az storage entity insert --account-name $StorageAccountName --table-name $TableNameReleases --if-exists merge --entity PartitionKey=$User RowKey=$ReleaseName MaxAllowedDownloads=$MaxAllowedDownloads Url=$fileName MaxAllowedDownloads@odata.type=Edm.Int32 

# Upload file
az storage blob upload --account-name $StorageAccountName --container-name $ContainerNameFiles --file $Path --name $fileName 

# Create codes
$codes = @()
for ($i = 0; $i -lt $NumberOfCodes; $i++) {
    $code = "$CodePrefix$(Get-RandomString)"
    $codes += $code
    az storage entity insert --account-name $StorageAccountName --table-name $TableNameCodes --if-exists merge --entity PartitionKey=$ReleaseName RowKey=$code
}

$codes
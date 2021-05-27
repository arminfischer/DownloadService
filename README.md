# Download Service

## Summary
This project provides a simple but effective Download Service.

## Components
* Azure Function which finds Blob file for given release and code, optionally checks if the number of downloads has exceeded the allowed number of downloads and creates a short temporary URL to download the file
* Azure Storage
  * Blob container with downloadable files
  * Table "Releases" for releases pointing to blob files
  * Table "Codes" for codes for each release

## Deploy
* Install Azure CLI.
* Log on to Azure via `az login`.
* Run `cicd/deploy.ps1`.

## Verify
...

## Future Enhancements
* Github action to create 
* PowerShell to create releases
* UI and backend to manage releases
* ...

## Notes
* Full connection string with Storage Account Key required in order to generate SAS key. This is not ideal for security, one improvement could be to use a locked down Key Vault.
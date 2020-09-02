@echo off

set /p domain="AD Domain: "
dotnet user-secrets set "AzureAd:Domain" %domain% --project "..\..\src\Events.Server"

set /p clientid="AD Client Id: "
dotnet user-secrets set "AzureAd:ClientId" %clientid% --project "..\..\src\Events.Server"

set /p tenantid="AD Tenant Id: "
dotnet user-secrets set "AzureAd:TenantId" %tenantid% --project "..\..\src\Events.Server"

pause 
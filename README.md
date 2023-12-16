# create db:
1. dotnet ef migrations add --project QuickJob.DataModel --startup-project QuickJob.Api --verbose {Migration name}
2. dotnet ef database update --project QuickJob.DataModel --startup-project QuickJob.Api --verbose {Migration name}

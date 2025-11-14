	# .NET 10.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 10.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 10.0 upgrade.
3. Upgrade ScrewGameCard.Domain\ScrewGameCard.Domain.csproj
4. Upgrade ScrewGameCard.Application\ScrewGameCard.Application.csproj
5. Upgrade ScrewGameCard.ServiceDefaults\ScrewGameCard.ServiceDefaults.csproj
6. Upgrade ScrewGameCard.Infrastructure\ScrewGameCard.Infrastructure.csproj
7. Upgrade ScrewGameCard.HttpApi.Host\ScrewGameCard.HttpApi.Host.csproj
8. Upgrade ScrewGameCard.Portal\ScrewGameCard.Portal.esproj
9. Upgrade ScrewGameCard.AppHost\ScrewGameCard.AppHost.csproj

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name                                   | Description                 |
|:-----------------------------------------------|:---------------------------:|


### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                                     | Current Version | New Version | Description                                   |
|:-------------------------------------------------|:---------------:|:-----------:|:----------------------------------------------|
| Microsoft.EntityFrameworkCore.Tools              |   9.0.10        | 10.0.0      | Recommended update for .NET 10.0              |
| Microsoft.Extensions.Configuration.Abstractions  |   8.0.0         | 10.0.0      | Update to match .NET 10.0 runtime libraries   |
| Microsoft.Extensions.DependencyInjection.Abstractions |   8.0.2    | 10.0.0      | Update to match .NET 10.0 runtime libraries   |
| Microsoft.Extensions.Http.Resilience            |   9.9.0         | 10.0.0      | Update to compatible 10.0 package             |
| Microsoft.Extensions.ServiceDiscovery           |   9.5.0         | 10.0.0      | Update to compatible 10.0 package             |
| OpenTelemetry.Instrumentation.AspNetCore        |   1.9.0         | 1.14.0      | Recommended OpenTelemetry update for .NET 10  |
| OpenTelemetry.Instrumentation.Http              |   1.9.0         | 1.14.0      | Recommended OpenTelemetry update for .NET 10  |


### Project upgrade details
This section contains details about each project upgrade and modifications that need to be done in the project.

#### ScrewGameCard.Domain\ScrewGameCard.Domain.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net10.0`

NuGet packages changes:
  - No NuGet package updates were reported for this project.

Other changes:
  - Ensure APIs and runtime behaviors are compatible with .NET 10.


#### ScrewGameCard.Application\ScrewGameCard.Application.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net10.0`

NuGet packages changes:
  - `Microsoft.Extensions.Configuration.Abstractions` should be updated from `8.0.0` to `10.0.0`.
  - `Microsoft.Extensions.DependencyInjection.Abstractions` should be updated from `8.0.2` to `10.0.0`.

Other changes:
  - Review code for any API breaking changes related to updated Microsoft.Extensions packages.


#### ScrewGameCard.ServiceDefaults\ScrewGameCard.ServiceDefaults.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net10.0`

NuGet packages changes:
  - `Microsoft.Extensions.Http.Resilience` should be updated from `9.9.0` to `10.0.0`.
  - `Microsoft.Extensions.ServiceDiscovery` should be updated from `9.5.0` to `10.0.0`.
  - `OpenTelemetry.Instrumentation.AspNetCore` should be updated from `1.9.0` to `1.14.0`.
  - `OpenTelemetry.Instrumentation.Http` should be updated from `1.9.0` to `1.14.0`.

Other changes:
  - Verify resilience and service discovery behavior with new package versions.


#### ScrewGameCard.Infrastructure\ScrewGameCard.Infrastructure.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net10.0`

NuGet packages changes:
  - No NuGet package updates were reported for this project.

Other changes:
  - Validate platform-specific APIs and dependencies.


#### ScrewGameCard.HttpApi.Host\ScrewGameCard.HttpApi.Host.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net10.0`

NuGet packages changes:
  - `Microsoft.EntityFrameworkCore.Tools` should be updated from `9.0.10` to `10.0.0` (development tools package, ensure EF Core runtime compatibility separately).

Other changes:
  - Update Swagger/OpenAPI and hosting checks if needed for new runtime.


#### ScrewGameCard.Portal\ScrewGameCard.Portal.esproj modifications

Project properties changes:
  - Target framework should be changed from `net6.0` to `net10.0`

NuGet packages changes:
  - No NuGet package updates were reported for this project in analysis output; review project file for additional dependencies.

Other changes:
  - This project currently targets net6.0; more thorough API compatibility checks are recommended.


#### ScrewGameCard.AppHost\ScrewGameCard.AppHost.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net10.0`

NuGet packages changes:
  - No NuGet package updates were reported for this project.

Other changes:
  - Validate host configuration and runtime behaviors.



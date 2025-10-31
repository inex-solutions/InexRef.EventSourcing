# .NET8.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET8.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET8.0 upgrade.
3. Upgrade Rob.EventSourcing.Contracts\Rob.EventSourcing.Contracts.csproj
4. Upgrade Rob.EventSourcing\Rob.EventSourcing.csproj
5. Upgrade Rob.EventSourcing.Sql\Rob.EventSourcing.Sql.csproj
6. Upgrade Rob.EventSourcing.Tests\Rob.EventSourcing.Tests.csproj

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name | Description |
|:-----------------------------------------------|:---------------------------:|
| None | No projects excluded |

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name | Current Version | New Version | Description |
|:------------------------------------|:---------------:|:-----------:|:----------------------------------------------|
| Newtonsoft.Json |10.0.3 |13.0.4 | Security vulnerability — update to secure version and latest supported for .NET8.0 |
| System.Reflection |4.3.0 | | Functionality included in .NET8.0 — remove package and use framework reference |

### Project upgrade details
This section contains details about each project upgrade and modifications that need to be done in the project.

#### Rob.EventSourcing.Contracts modifications

Project properties changes:
 - Convert project file to SDK-style.
 - Target framework should be changed from `\n.NETFramework,Version=v4.8` to `net8.0`.

NuGet packages changes:
 - No NuGet package updates detected for this project.

Feature upgrades:
 - Convert any old MSBuild imports or targets to SDK-style equivalents.

Other changes:
 - Ensure assembly info, strong-name, or other legacy properties are migrated to SDK-style properties if needed.

#### Rob.EventSourcing modifications

Project properties changes:
 - Convert project file to SDK-style.
 - Target framework should be changed from `\n.NETFramework,Version=v4.8` to `net8.0`.

NuGet packages changes:
 - `Newtonsoft.Json` should be updated from `10.0.3` to `13.0.4` (security vulnerability).
 - Remove `System.Reflection` package (4.3.0) because its functionality is provided by the target framework.

Feature upgrades:
 - Review any APIs that differ between .NET Framework and .NET8 (e.g., System.Web, configuration, remoting, WCF) and replace or remove unsupported APIs.

Other changes:
 - Update `using` and references to reflect renamed or moved assemblies if compilation errors appear after upgrade.

#### Rob.EventSourcing.Sql modifications

Project properties changes:
 - Convert project file to SDK-style.
 - Target framework should be changed from `\n.NETFramework,Version=v4.8` to `net8.0`.

NuGet packages changes:
 - `Newtonsoft.Json` should be updated from `10.0.3` to `13.0.4` (security vulnerability).

Feature upgrades:
 - Validate any SQL-related runtime dependencies and update connection-related code that may target Windows-specific APIs.

Other changes:
 - Ensure native or platform-specific libraries are available for .NET8 or replaced with cross-platform alternatives.

#### Rob.EventSourcing.Tests modifications

Project properties changes:
 - Convert project file to SDK-style.
 - Target framework should be changed from `\n.NETFramework,Version=v4.8` to `net8.0`.

NuGet packages changes:
 - No specific NuGet package updates detected by analysis; update test framework packages if they are incompatible with .NET8.0.

Feature upgrades:
 - Migrate test project to use `Microsoft.NET.Test.Sdk` and modern test adapters/runners compatible with .NET Core/.NET8 if needed.

Other changes:
 - Update assembly scanning or test initialization code that relies on .NET Framework-only features.

# .NET 8.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that a .NET 8.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 8.0 upgrade.
3. Upgrade BetterJoyForCemu\BetterJoy.csproj to .NET 8.0

## Settings

This section contains settings and data used by execution steps.

### Project upgrade details

This section contains details about each project upgrade and modifications that need to be done in the project.

#### BetterJoyForCemu\BetterJoy.csproj modifications

Project conversion:
  - Convert project from .NET Framework style to SDK-style project format

Project properties changes:
  - Target framework should be changed from `net48` to `net8.0-windows`
  - OutputType should be set to `WinExe` for Windows Forms application
  - UseWindowsForms should be set to `true`

Code changes required:
  - Replace `System.Configuration.ConfigurationManager` with `Microsoft.Extensions.Configuration` or add NuGet package `System.Configuration.ConfigurationManager`
  - Update Windows Forms initialization code to use modern .NET patterns
  - Review and update P/Invoke declarations if any
  - Replace `app.config` configuration with modern configuration patterns (JSON, etc.)

Other changes:
  - Remove `packages.config` file (if exists) - dependencies will be managed in the project file
  - Update `.gitignore` to exclude `bin/` and `obj/` folders for SDK-style projects
  - Review and update any post-build events or custom MSBuild tasks

# GraphiteUi

GraphiteUi is a Blazor UI component library for SSR and interactive rendering modes.

## Package source setup (GitHub Packages)

Create or update `nuget.config` with the GitHub Packages source and credentials:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="github-graphiteui" value="https://nuget.pkg.github.com/IvanDmitriev1/index.json" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <github-graphiteui>
      <add key="Username" value="YOUR_GITHUB_USERNAME" />
      <add key="ClearTextPassword" value="YOUR_GITHUB_PAT" />
    </github-graphiteui>
  </packageSourceCredentials>
</configuration>
```

Use a PAT with `read:packages` scope (and `repo` if your package visibility/repo access requires it).

## Install

```bash
dotnet add package GraphiteUi --source "https://nuget.pkg.github.com/IvanDmitriev1/index.json"
```

## Basic setup

```csharp
using GraphiteUI.Extensions;

builder.Services.AddGraphiteUi();
```

## Minimal usage

```razor
<UiButton Color="ThemeColor.Primary">Click me</UiButton>
```

Repository and docs: https://github.com/IvanDmitriev1/GraphiteUi

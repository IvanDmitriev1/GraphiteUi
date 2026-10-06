# GraphiteUi

GraphiteUi is a Blazor UI component library for static SSR and interactive rendering modes.

## Package feeds

GitHub Actions publishes to two feeds:

- **NuGet.org:** pushing a tag matching `v*` publishes the tagged commit using the project's original `Version`, including a prerelease suffix. For example, push `v0.5.7-preview.1` for a commit whose project version is `0.5.7-preview.1`. Merging a pull request alone does not publish to NuGet.org.
- **GitHub Packages:** pushes to `master` or `develop` publish a development package using `<major.minor.patch>-dev.<GITHUB_RUN_NUMBER>`. The workflow strips prerelease and build metadata from the evaluated project `Version`; for example, `0.5.7-preview.1` becomes `0.5.7-dev.42`. Direct pushes to `master` publish only this development package.

Install the NuGet.org package:

```bash
dotnet add package GraphiteUi
```

For the current prerelease, request its version explicitly:

```bash
dotnet add package GraphiteUi --version 0.5.7-preview.1
```

To install a development build from GitHub Packages, add the source and credentials to `nuget.config`:

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

Use a GitHub token with `read:packages` (and `repo` if repository access is required), then install with:

```bash
dotnet add package GraphiteUi --source "https://nuget.pkg.github.com/IvanDmitriev1/index.json" --prerelease
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

## Styles

Add this before your application stylesheet:

```html
<link rel="stylesheet" href="_content/GraphiteUi/css/graphite-ui.css" />
```

The package ships precompiled component utilities without a global CSS reset. Normal consumers do not need Node or Tailwind. For custom Tailwind utility classes, build consumer CSS as usual. Override GraphiteUi CSS primitives in app CSS to retune the palette. JS initializers are loaded by Blazor automatically.

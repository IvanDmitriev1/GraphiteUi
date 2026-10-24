# GraphiteUi

GraphiteUi is a Blazor UI component library for static SSR and interactive rendering modes.

## Package feeds

GitHub Actions publishes to two feeds:

- **NuGet.org:** pushing a tag matching `v*` publishes only if the tagged commit belongs to `master` (its current tip or an ancestor). The package uses the project's original `Version`, including a prerelease suffix. For example, push `v0.5.7-preview.1` for a commit on `master` whose project version is `0.5.7-preview.1`. Merging a pull request alone does not publish to NuGet.org.
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

Generate application CSS with Tailwind during the server build. `GraphiteUi.targets` copies the library CSS files to `bin/GraphiteUi/Styles` by default. Import the library stylesheet after Tailwind in an entrypoint at `Styles/app.css`:

```css
@import "tailwindcss";
@import "../bin/GraphiteUi/Styles/graphite-ui.css";
@source "../Components";
```

Import the entrypoint after the consumer's Tailwind import and scan the consumer's own Razor sources as needed. GraphiteUi styles use `@apply` in the `components` layer; the library CSS entrypoint imports its theme and per-family styles and does not import Tailwind or scan source files. Link the generated application stylesheet from the host page. The docs application's build targets provide a working example. Override GraphiteUi CSS primitives in app CSS to retune the palette. JS initializers are loaded by Blazor automatically.

# GraphiteUi

Blazor UI component library targeting .NET 10, with support for static SSR and interactive rendering.

- [docs/API.md](docs/API.md) - component and parameter reference, services, theme tokens, and known gaps.
- [AGENTS.md](AGENTS.md) - contributor playbook and conventions.

## Builds and packages

GitHub Actions restores and builds the solution in Release on every branch push and pull request update. Package publishing uses two feeds:

- **GitHub Packages development builds:** pushes to `master` or `develop` publish `<major.minor.patch>-dev.<GITHUB_RUN_NUMBER>`. The major, minor, and patch parts come from the evaluated `Version` in `GraphiteUi/GraphiteUi.csproj`; any prerelease or build metadata is removed. For example, `0.5.7-preview.1` publishes as `0.5.7-dev.42`. A direct push to `master` publishes only this development package.
- **NuGet.org releases:** pushing a tag matching `v*` publishes only if the tagged commit belongs to `master` (its current tip or an ancestor). The package uses the original project `Version`, including any prerelease suffix. For example, push `v0.5.7-preview.1` for a commit on `master` whose project version is `0.5.7-preview.1`. Merging a pull request alone does not publish to NuGet.org.

Install the NuGet.org package:

```bash
dotnet add package GraphiteUi
```

To install the current NuGet prerelease explicitly, use:

```bash
dotnet add package GraphiteUi --version 0.5.7-preview.1
```

Development builds are available from `https://nuget.pkg.github.com/IvanDmitriev1/index.json` and require a GitHub token with `read:packages` (and `repo` if repository access is required).

```bash
dotnet add package GraphiteUi --source "https://nuget.pkg.github.com/IvanDmitriev1/index.json" --prerelease
```

### Publishing setup

Configure a NuGet.org trusted publishing policy for GitHub Actions with owner `IvanDmitriev1`, repository `GraphiteUi`, workflow file `publish-nuget.yml`, an empty environment, and package scope `GraphiteUi`. In GitHub repository settings, under **Secrets and variables > Actions**, add:

- `NUGET_USER`: the NuGet.org profile username (not the email address) of the person who created the trusted publishing policy.
- `GH_PACKAGES_TOKEN`: the existing GitHub personal access token with `write:packages` (and `repo` if required by package visibility).

The NuGet.org job uses OIDC through `NuGet/login@v1`; it does not need a long-lived NuGet API key.

The library ships its component styles and JavaScript as package assets. Consumer applications need only .NET; the docs app uses the standalone Tailwind CLI target.

# GraphiteUi

Blazor UI component library targeting .NET 10 with support for static SSR and interactive rendering.

- [docs/API.md](docs/API.md) - API reference: components and parameters, dialog/toast services, theme tokens, known gaps.
- [AGENTS.md](AGENTS.md) - contributor playbook and conventions.

## Release flow (dev packages)

1. Update `<Version>` in `GraphiteUi/GraphiteUi.csproj` (base version only, for example `0.5`).
2. Push to `master` or `develop`.
3. GitHub Actions publishes a dev package to GitHub Packages using version:
   - `<Version>-dev.<GITHUB_RUN_NUMBER>`
   - Example: `0.5-dev.42`

### Publish target

- GitHub Packages only: `https://nuget.pkg.github.com/<owner>/index.json`
- Required GitHub Actions secret: `GH_PACKAGES_TOKEN`

## ParcelStorage adaptation

See [docs/parcelstorage.md](docs/parcelstorage.md). Component CSS is now included
as a Razor static web asset. To regenerate it, run `npm ci && npm run build:css`.
The docs app downloads the standalone Tailwind CSS CLI through `Tailwindcss.targets`; its CSS build does not require Node.js or npm packages.
Normal consumers only need .NET.

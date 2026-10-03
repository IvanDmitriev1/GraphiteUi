# ParcelStorage integration

Local version: `0.5.6-parcelstorage.2`, based on commit `e698e63`.
The integration targets Blazor Interactive Server in .NET 10. The library remains
independent of ParcelStorage services, authentication, database and identifier types.
Nothing has been published to GitHub Packages.

## Adaptations

- Precompiled library CSS is shipped at `_content/GraphiteUi/css/graphite-ui.css`.
  It contains semantic tokens and component utilities without global preflight.
  A scoped base reset normalizes only component elements with `data-slot`.
  It is checked in, so consumer builds do not require Node or CSS tooling.
- Debounce uses one cancellable delay for each pending input. Superseded input
  and disposal are normal cancellation paths. Disabled/read-only fields do not commit.
- Textbox/Numbox fields link labels, IDs, description and validation state to the
  actual input. Caller accessibility labels and input modes are preserved.
  `Size` determines their height. Textbox type updates after render.
- Numbox preserves caller `step`/`inputmode`, accepts invariant or current-culture
  decimals and does not mistake a comma decimal separator for thousands grouping.
- Popup honors trigger tag/mode and per-slot classes/attributes. A button trigger
  explicitly uses `type="button"`, so opening it does not submit a parent form.
  Closed content is inert and hidden from accessibility APIs; closing transitions
  finish before `hidden` is applied, and pending timers are cancelled on reopen/disposal.
- Select supports nullable typed option values without a ParcelStorage dependency.
  The input ID is not duplicated on the popup root. Items re-register on parameter
  updates, and disabled items/clear actions cannot change the value.
- Select fills its trigger width and exposes combobox/listbox relationships.
  Query filtering honors `hidden`; closing restores the committed display text.
  Enter does not submit forms, and clear defers its disabled state until the click
  reaches Blazor's document delegate, so the bound value is actually reset.
- Dialog, alert, badge, button and checkbox roots use scoped box sizing.
  Dialog padding stays within the viewport, descriptions avoid browser margins,
  and checkbox captions can wrap. Link buttons do not inherit underlines.
- TwMerge recognizes Graphite typography sizes independently from text colors,
  preserving semantic foreground classes when consumer classes are appended.
- `ButtonType.Reset` maps to native `reset`. This does not reset a Blazor model;
  consumers that need a model reset should use an explicit handler.
- Theme copying preserves directories. Docs CSS builds before static asset
  discovery and uses the pinned Node CLI instead of an OS-specific auto-download.

## Build and validate

```bash
npm ci
npm run build:css
dotnet build GraphiteUi.sln
dotnet run --project tests/GraphiteUi.RegressionTests
dotnet pack GraphiteUi/GraphiteUi.csproj -c Release -o artifacts/packages
```

Node is needed to regenerate CSS and build the docs app, not to build the RCL or
ParcelStorage from checked-in CSS. Tailwind packages are pinned in `package-lock.json`.
The automated checks cover static rendering, label association, invalid state,
merged description references, numeric precision, typed select markup, popup
slots, debounce cancellation/disposal, Russian decimal input and typography/color merging.

Interactive Server behavior is verified in ParcelStorage's Development-only
`/ui-preview`: select/clear typed IDs, validation, submit, dialog confirmation and
notifications. Browser verification covers static SSR at `/compatibility-static`
and Interactive WebAssembly at `/compatibility-wasm` in the docs app, including
open/Escape/close behavior, hidden/inert state and a WASM callback. All three
rendering modes complete these checks without browser warnings or errors.

## Remaining component boundaries

Select's unregister bookkeeping, Drawer open-state/Escape handling and
consumer class merging on input wrappers remain as documented in `API.md`.
ParcelStorage keeps native selects and storage radio options for its
working forms while adopting the library's buttons, text/numeric fields, badges
alerts and checkboxes. Its table/layout CSS is intentionally application-owned.

A local package feed can be produced without publishing. From ParcelStorage:

```bash
dotnet pack ../GraphiteUi/GraphiteUi/GraphiteUi.csproj -c Release -o .artifacts/graphiteui
dotnet restore src/ParcelStorage.Web -p:UseGraphiteUiPackage=true \
  --source .artifacts/graphiteui --source https://api.nuget.org/v3/index.json
dotnet build src/ParcelStorage.Web -p:UseGraphiteUiPackage=true --no-restore
```

For normal collaboration ParcelStorage references the neighboring source clone;
`GraphiteUiProject` can override that path. Upgrading requires syncing the source
and pinned package version rather than silently falling back to an older package.

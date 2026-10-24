# GraphiteUi - Agent Playbook

Field manual for contributors to the GraphiteUi Blazor UI library (targets .NET 10). Keep changes fast, safe, and compatible with static SSR + client enhancement, Interactive Server, and Interactive WebAssembly.

## Scope & Goals
- Ship cohesive, high-quality components with minimal allocations and predictable performance.
- Support all rendering modes: static SSR, Interactive Server, Interactive WebAssembly.
- Keep styling centralized in CSS and make consumer classes predictable to override.

## Design Principles (Required)
- Apply GRASP when introducing behavior:
  - Information Expert: keep logic near the component/service that owns the data.
  - Controller: coordinate workflows in services/host components, not leaf UI elements.
  - Low Coupling and High Cohesion: keep features focused and isolated.
  - Polymorphism and Pure Fabrication: use abstractions/base types when behavior varies.
- Follow SOLID:
  - Single Responsibility: each component/style/service has one clear reason to change.
  - Open/Closed: extend with parameters/composition/new types; avoid breaking edits.
  - Liskov Substitution: derived/base UI types preserve contracts and behavior.
  - Interface Segregation: prefer narrow interfaces for specific scenarios.
  - Dependency Inversion: depend on abstractions (for example `IDialogService`, `IToastService`) and wire through DI.
- Keep KISS as default:
  - Choose the simplest design that meets SSR + interactive requirements.
  - Avoid speculative abstractions, unnecessary indirection, and premature optimization.

## Repo Map (what to touch)
- `docs/API.md` - public API reference (components, parameters, services, theme tokens, known gaps). Read before writing GraphiteUi markup.
- `GraphiteUi/` - Razor Class Library.
- `GraphiteUi/Components/**` - components (`Ui*.razor` + `.razor.cs`), base types in `Components/Bases`.
- `GraphiteUi/Styles/*.css` - centralized theme and component-family styles.
- `GraphiteUi/Styles/graphite-ui.css` - library stylesheet entrypoint.
- `GraphiteUi/wwwroot/js/*.js` - interop modules.
- `GraphiteUi/wwwroot/GraphiteUi.lib.module.js` - JS boot hooks for web start/enhanced navigation.
- `GraphiteUi/GraphiteUi.targets` - copies theme assets to consumer output.
- `GraphiteUi/Extensions/ServiceCollectionExtensions.cs` - `AddGraphiteUi` DI entrypoint.
- `GraphiteUi.Docs/**` - optional docs/examples app.
- `GraphiteUi.Benchmarks/**` - performance experiments.

## Build & Tooling
- Prerequisite: .NET 10 SDK.
- Required before merge: `dotnet build GraphiteUi.sln`
- Optional: build docs app when UX/samples changed.
- Library CSS sources are checked in; generated docs CSS is build output. No required formatting tool.

## Component Conventions
- Use paired files: `UiThing.razor` + `UiThing.razor.cs`.
- Prefix component names with `Ui`.
- Base types:
  - `UiComponentBase` for general components.
  - `UiInputBase<TValue>` and `UiInputFieldBase<TValue>` for form inputs.
- Respect and pass through common parameters: `As`, `Class`, `AdditionalAttributes`, `ChildContent`, `Disabled`, `ReadOnly`, `Required`.
- Ensure `ElementReference` is assigned in render tree paths that require JS hooks.
- Keep accessibility attributes consistent (`aria-disabled`, `aria-invalid`, `aria-expanded`, labels).
- Use `ToAttributeValue()` for boolean data/aria attributes when string values are needed.

## Styling Rules
- Keep component appearance in one `Styles/_<family>.css` file per component family. Compose Tailwind utilities with `@apply` inside `@layer components` and use `:where([data-ui=...])` roots plus scoped native nesting for low-specificity styling.
- Put stable `data-ui` identities on roots after attribute splats; identify owned descendants with `data-part`. Preserve existing `data-slot` and JS state attributes when they are part of the runtime contract.
- Normalize finite enum values to lowercase kebab-case data attributes and booleans to `true`/`false`; CSS owns appearance and fallback values.
- Keep all visual roles in CSS. Use semantic role tokens such as `bg-surface-hover`, `text-muted-foreground`, `border-control`, and `bg-control-checked`; bind new roles in `Styles/_theme.css` and theme values in `_dark.css`.
- `Class` and component part-class parameters pass through unchanged. Keep their selectors low specificity so consumer utilities can override the library appearance without a merge service.
- `Styles/graphite-ui.css` imports the library CSS files; consumers import it after their own `tailwindcss` import. Do not scan library C#, Razor, or component files from the entrypoint.

## JS and Rendering Modes
- Put JS modules in `GraphiteUi/wwwroot/js`.
- Load modules with `JsRuntime.LoadModule("js/<file>.js")`.
- Expose both document-level `refresh*` and element-level `*Blazor` functions.
- JS setup must be idempotent (init flags, no duplicate handlers after rerender/enhanced navigation).
- For new interactive widgets, register boot refresh in `GraphiteUi.lib.module.js` (`beforeWebStart`, `afterWebStarted`).
- Perform interop in `OnAfterRenderAsync(firstRender)`.
- Avoid JS interop in `OnInitialized`.
- Guard long-lived JS object disposal/invocation for disconnect scenarios (`JSDisconnectedException`).
- Static SSR should always produce usable markup; client JS enhances behavior after load.

## JS Code Style Standard
- Follow `GraphiteUi/wwwroot/js/JS_CODE_STYLE.md` for all JS interop modules.
- Keep export names verb-first (`refresh*`, `setup*`, `destroy*`) and prefer camelCase for canonical APIs.
- JS modules own listener/timer lifecycle and must remain idempotent across rerender/enhanced navigation.
- For JS interop element state, prefer module `WeakMap` storage over custom DOM properties.
- Compatibility aliases are allowed when renaming interop exports.

## Performance and Allocation Discipline
- Avoid per-render string interpolation for attributes when a direct enum/boolean mapping is enough.
- Use finite normalized data-attribute values and CSS selectors for appearance variants.
- Minimize unnecessary `StateHasChanged` calls.
- Use `UiDebouncedInputBase` for debounced input behavior where applicable.

## Theme and Assets
- Keep semantic token changes in `_theme.css` and `_dark.css`.
- If theme asset paths change, update packing/copy rules in `GraphiteUi.csproj` and `GraphiteUi.targets`.
- Consumers rely on `GraphiteUi.targets` for `theme/Styles/*.css` copy behavior into `bin/GraphiteUi` (or `GraphiteUiThemeDir`).

## Services and DI
- Keep DI registration centralized in `AddGraphiteUi` (`GraphiteUI.Extensions`).
- `AddGraphiteUi()` registers `IDialogService` and `IToastService`.
- Register new cross-cutting services in the same extension method.

## Docs and Examples
- Docs updates are optional but encouraged when UX or API behavior changes.
- Keep docs imports and casing aligned with library namespaces/components.
- Follow `RAZOR_COMPONENT_STYLE.md` for detailed Razor component authoring guidance and performance review targets.

## PR Checklist
- [ ] Run `dotnet build GraphiteUi.sln`
- [ ] If JS/interop changed, verify static SSR + Interactive Server + Interactive WebAssembly behavior.
- [ ] If JS changed, verify `GraphiteUi/wwwroot/js/JS_CODE_STYLE.md` compliance and lifecycle/idempotence behavior.
- [ ] Ensure accessibility and keyboard behavior remains correct.
- [ ] Update docs/examples when useful (optional).

## Common Pitfalls
- Forgetting `ElementReference` assignment for JS-driven components.
- Putting complex style strings directly in `.razor` files.
- Non-idempotent JS initialization causing duplicate listeners.
- Missing boolean attribute normalization (`ToAttributeValue()`).
- Adding new interop modules without wiring `GraphiteUi.lib.module.js` refresh hooks.


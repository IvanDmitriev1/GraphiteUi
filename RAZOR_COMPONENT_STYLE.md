# GraphiteUi Razor Component Style Guide

Best-practice guide for authoring Razor components in GraphiteUi with a performance-first mindset.

## Scope and Relationship
- This guide covers component authoring across:
  - `GraphiteUi/Components/*.razor`
  - `GraphiteUi/Components/*.razor.cs`
  - `GraphiteUi/Styles/*.cs`
- This document complements `AGENTS.md` and `GraphiteUi/wwwroot/js/JS_CODE_STYLE.md`.
- Use this guide for implementation details and review consistency without duplicating broader repo policies.
- Guidance is best-practice oriented.
- Numeric targets in this document are review budgets, not CI blockers.

## Core Principles
1. SSR-first markup: components must render usable HTML before client enhancement.
2. Least DOM necessary: avoid wrappers that exist only for convenience.
3. Least allocations per render: keep render-path memory pressure low and predictable.

## Component Structure
- Use paired files (`UiThing.razor` + `UiThing.razor.cs`) and keep naming consistent with existing `Ui*` components.
- Prefer direct-child structure when possible.
- Avoid extra containers used only for spacing or grouping when styles can be applied to existing elements.
- DOM nesting target:
  - Default target: `<= 4` levels.
  - Exception target: `5` levels only with explicit PR rationale.
- Keep styling composition in `Styles/*.cs`; keep `.razor` markup focused on structure and attributes.

## Memory and Allocation Discipline
- Do not build class strings with per-render interpolation in markup.
- Compose classes with `CssClassBuilder` and size stackalloc buffers to expected output.
- Merge consumer class overrides through `MergeRootClass`; return static core classes directly when no override exists.
- Avoid per-render `new Dictionary<>` / `new List<>` in hot paths.
- Avoid avoidable lambda captures in frequently rendered markup/event wiring.
- Precompute variant class tables in style classes when combinations are finite.
- Keep `OnParametersSet` work incremental and cache derived values when inputs are unchanged.

## Razor Markup Rules
- Use semantic HTML that matches intent (`button`, `label`, `input`, `nav`, `dialog`, etc.).
- Keep markup SSR-usable without requiring JavaScript for baseline functionality.
- Pass through common parameters/attributes consistently (`Class`, `AdditionalAttributes`, `ChildContent`, `Disabled`, `ReadOnly`, `Required` as applicable).
- Keep stable `data-slot` naming for elements that styles or JS depend on.
- Use `ToAttributeValue()` when boolean values must be represented as string attributes (`data-*`, `aria-*`).
- Prefer simple ultra-cheap expression paths directly in markup when logic is trivial (for example `Type.ToHtmlValue()`).
- Keep root element responsibilities clear: attributes, classes, and accessibility metadata should be predictable.

## Code-Behind Rules
- Keep component responsibilities narrow and cohesive.
- `OnParametersSet` should be cheap, deterministic, and idempotent.
- Prefer deriving presentation state from parameters instead of mutating long-lived state unnecessarily.
- Call `StateHasChanged` only when a rerender is actually required.
- Dispose resources deterministically (`CancellationTokenSource`, timers, subscriptions, JS references).
- Use `UiDebouncedInputBase<TValue>` for debounced input behavior instead of ad-hoc debounce logic.

## Interop and Rendering Modes
- Render valid, usable HTML for static SSR first; JS should enhance behavior after load.
- Perform JS interop in `OnAfterRenderAsync(firstRender)`.
- Do not perform JS interop in `OnInitialized`.
- Ensure interop setup is idempotent across rerenders and enhanced navigation.
- Assign and preserve `ElementReference` in render paths that require interop hooks.
- Guard long-lived interop disposal/invocation for disconnect scenarios (`JSDisconnectedException`).

## Accessibility and Interaction
- Preserve keyboard parity for pointer interactions.
- Ensure visible focus states for interactive elements.
- Keep `aria-*` attributes accurate (`aria-disabled`, `aria-invalid`, `aria-expanded`, labels).
- Preserve expected disabled/read-only behavior in both semantics and visuals.
- Do not remove label associations when reducing nesting.

## PR Review Rubric
- [ ] Component structure follows direct-child-first approach and avoids convenience wrappers.
- [ ] Nesting depth target is met (`<= 4`) or `5`-depth usage includes PR rationale.
- [ ] No avoidable render-path allocations (dynamic class interpolation, repeated collection allocation).
- [ ] Class composition is centralized in `Styles/*.cs` and merged via `MergeRootClass` with bounded TwMerge caching.
- [ ] Interop setup is idempotent and does not create duplicate handlers/listeners.
- [ ] SSR baseline behavior remains usable.
- [ ] Accessibility semantics and keyboard behavior remain correct.
- [ ] For component-only changes, run `dotnet build GraphiteUi/GraphiteUi.csproj`.

## Examples

```csharp
using GraphiteUi.Utilities;

namespace GraphiteUi.Styles;

public static class TagStyles
{
    public static string RootClass { get; } = new CssClassBuilder(stackalloc char[128])
        .Add("inline-flex")
        .Add("items-center")
        .Add("rounded-md")
        .ToString();
}
```

### 1) Good input component split
- Keep `.razor` focused on structure and binding.
- Keep `.razor.cs` focused on parsing/state transitions.
- Keep classes and variants in `Styles/*.cs` and merge root class via `MergeRootClass`.

### 2) Avoid pattern
```razor
<!-- Avoid: unnecessary wrapper nesting and per-render allocations -->
<div class="@($"p-2 {Class}")">
    <span>
        <span>
            <input @attributes="@(new Dictionary<string, object> { ["data-x"] = "1" })" />
        </span>
    </span>
</div>
```

Use:
- flat structure where possible
- static/precomputed class composition in style classes
- cached or parameter-derived attributes without repeated allocations

## Appendix: Quick Audit Commands
```powershell
# Per-render class interpolation in Razor
rg -n 'class="@\\$\\(' GraphiteUi/Components

# Deep wrapper patterns worth review
rg -n '<span[^>]*>\\s*<span|<div[^>]*>\\s*<div' GraphiteUi/Components

# Potential repeated allocations in components
rg -n 'new Dictionary<|new List<' GraphiteUi/Components

# Manual rerender calls (verify necessity)
rg -n 'StateHasChanged\\(' GraphiteUi/Components

# Lifecycle/interop touchpoints for review
rg -n 'OnAfterRenderAsync|OnInitialized|JSDisconnectedException' GraphiteUi/Components
```


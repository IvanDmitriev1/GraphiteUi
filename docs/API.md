# GraphiteUi — API Reference

Blazor Razor Class Library, `net10.0`, `PackageId GraphiteUi`.

- Works in **static SSR**, **Interactive Server**, and **Interactive WebAssembly**. Most components render usable markup under static SSR and are enhanced by JS after load.
- **Generate CSS during the consuming application's server build.** Import Tailwind first, then the copied `Styles/graphite-ui.css` entrypoint. It imports GraphiteUi's theme and per-family component CSS. The package ships CSS source files; generated application stylesheets are build output. The docs app's `Tailwindcss.targets` and `wwwroot/styles/app.css` demonstrate this pipeline.
- **No `<script>` tag needed.** `wwwroot/GraphiteUi.lib.module.js` is a Blazor JS initializer; `blazor.web.js` loads it automatically. It re-wires inputs, popups, radios, selects and tooltips on `enhancedload`.
- Namespaces: components/services in `GraphiteUi.Components` and enums in `GraphiteUi.Common`.

---

## 1. Wiring

```csharp
using GraphiteUI.Extensions;   // NOTE: capital "UI" — see Known gaps

builder.Services.AddGraphiteUi();
```

In the consuming Tailwind input, import the library entrypoint after Tailwind and scan the app's Razor sources:

```css
@import "tailwindcss";
@import "../../bin/GraphiteUi/Styles/graphite-ui.css";
@source "../../../MyApp.Client/**/*.razor";
```

`GraphiteUi.targets` copies package CSS files to `bin/GraphiteUi/Styles` by default. Set `GraphiteUiThemeDir` to choose another copy directory and update the CSS import accordingly. Keep the entrypoint import after Tailwind and do not scan GraphiteUi source files from the consumer stylesheet.

Registers scoped `IDialogService` and `IToastService` services.
Call `AddGraphiteUi()` in *every* render-mode host project (server `Program.cs` **and** `.Client/Program.cs` for an Auto/WASM app).

`_Imports.razor`:

```razor
@using GraphiteUi.Components
@using GraphiteUi.Common
```

Layout — both hosts need an interactive render mode:

```razor
<DialogHost @rendermode="InteractiveServer" />
<UiToastHost @rendermode="InteractiveServer" />
```

Missing host throws at call time: `InvalidOperationException("No DialogHost present. Place <DialogHost /> in your layout.")` (same shape for `UiToastHost`).

Dark theme: `class="dark"` or `data-theme="dark"` on `<html>`. Light theme is the default.

---

## 2. Enums (`GraphiteUi.Common`)

| Enum | Members (in order) |
|---|---|
| `ThemeColor` | `Inherit`, `Primary`, `Secondary`, `Success`, `Warning`, `Danger`, `Info` |
| `Size` | `Small`, `Medium`, `Large` |
| `Align` | `Start`, `Center`, `End` |
| `AlignItems` | `None`, `Baseline`, `Center`, `Start`, `End`, `Stretch` |
| `ButtonType` | `Button`, `Submit`, `Reset` (native HTML reset semantics) |
| `ButtonVariant` | `Solid`, `Outline`, `Ghost`, `Link` (appearance only; `As` still selects the HTML element) |
| `InputType` | `Text`, `Password`, `Email` |
| `ToastPlacement` | `TopRight`, `TopCenter`, `BottomRight` |

---

## 3. Base types

```
ComponentBase
├── UiComponentBase : IUiComponent        ← all non-input components
│   └── UiComponent (sealed)              ← internal polymorphic primitive
├── UiBreadcrumbItem, UiRadio<T>, UiSelectItem<T>   ← plain ComponentBase, NO Class/As
├── BaseDialog / BaseDialog<TResult>
└── InputBase<TValue>                     (Blazor)
    └── UiInputBase<TValue> : IUiComponent
        ├── UiCheckbox, UiSwitch, UiFileInput, UiRadioGroup<T>
        └── UiDebouncedInputBase<TValue>
            └── UiInputFieldBase<TValue>   ← UiTextbox, UiNumbox<T>, UiSelect<T>
```

### `UiComponentBase` — inherited by every non-input component

| Member | Type | Default | Notes |
|---|---|---|---|
| `As` | `string` | `"div"` | Root HTML tag. Most components override in their constructor. |
| `Class` | `string?` | `null` | Passed to component roots unchanged. Component styles use low-specificity selectors so consumer utilities can override appearance. |
| `AdditionalAttributes` | `IReadOnlyDictionary<string, object>?` | `null` | `CaptureUnmatchedValues`, splatted on root. |
| `ElementReference` | `ElementReference?` | `null` | Public, `protected set`. Not a parameter. Only populated by `UiComponent` and `UiToastHost`. |
| `Rerender()` | `void` | — | Public. Calls `StateHasChanged()`. |

### `UiInputBase<TValue>` — `: InputBase<TValue>`. **No `As`, no `ElementReference`, no `Rerender()`.**

| Member | Type | Default |
|---|---|---|
| `Class` | `string?` | `null` |
| `Disabled` | `bool` | `false` |
| `ReadOnly` | `bool` | `false` |
| `Required` | `bool` | `false` |

Plus Blazor's `Value`, `ValueChanged`, `ValueExpression` (`@bind-Value`), `DisplayName`, `AdditionalAttributes`, cascading `EditContext`.
`protected internal bool IsInvalid` = `EditContext is not null && !EditContext.IsValid(FieldIdentifier)`.

### `UiDebouncedInputBase<TValue>`

| Member | Type | Default | Notes |
|---|---|---|---|
| `DebounceDelay` | `int` | `0` | Milliseconds. `0` = commit on every `oninput`. |
| `DebounceAsync(string?)` | `Task` | — | Public method. |

### `UiInputFieldBase<TValue>` — floating-label field shared by Textbox / Numbox / Select

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Label` | `string?` | `null` | Textbox/Numbox float it; Select shows it above the trigger. It also names the input. |
| `Placeholder` | `string?` | `null` | Also keeps Textbox/Numbox labels floated. |
| `Description` | `string?` | `null` | Rendered below Textbox/Numbox/Select and linked with `aria-describedby`. |
| `Size` | `Size` | `Medium` | Controls Textbox/Numbox height and Select trigger height (32/40/48px). |

Textbox/Numbox apply `Class` raw to their outer wrapper. `UiSelect` passes `Class` raw to its Popup root; the visible label and description sit in a field wrapper around it.

---

## 4. Component reference

### `UiAlert` — `As="section"`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Color` | `ThemeColor` | `Primary` | `Danger` switches `role` `status` → `alert`. |
| `Title` | `string?` | `null` | |
| `ChildContent` | `RenderFragment?` | `null` | Body. |
| `StartContent` | `RenderFragment?` | `null` | Leading icon slot. |
| `Dismissible` | `bool` | `false` | Renders the dismiss button. |
| `ShowDismissButton` | `bool` | `true` | When `false`, button stays in DOM but `hidden` + `tabindex="-1"`. |
| `Visible` | `bool` | `true` | `false` renders nothing. Supports `@bind-Visible`. |
| `VisibleChanged` | `EventCallback<bool>` | — | |
| `OnDismiss` | `EventCallback` | — | Fires after `VisibleChanged(false)`. |

### `UiAvatar` — `As="span"`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Src` | `string?` | `null` | Non-blank ⇒ renders `<img>`, otherwise initials. |
| `Alt` | `string` | `"Avatar"` | |
| `Name` | `string?` | `null` | Initials = first + last word initial, uppercased; `"?"` when empty. |
| `Size` | `Size` | `Medium` | |

### `UiBadge` — `As="span"`

| Parameter | Type | Default |
|---|---|---|---|
| `Color` | `ThemeColor` | `Primary` |
| `Size` | `Size` | **`Small`** (not `Medium`) |
| `ChildContent` | `RenderFragment?` | `null` |

### `UiBreadcrumb` — `As="nav"`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `AriaLabel` | `string` | `"Breadcrumb"` | |
| `ChildContent` | `RenderFragment?` | `null` | Placed inside an `<ol>`. |

### `UiBreadcrumbItem` — plain `ComponentBase` (**no `Class` / `As` / `AdditionalAttributes`**)

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Href` | `string?` | `null` | Renders `<a>` only when `!Current` and `Href` non-blank. |
| `Target` | `string?` | `null` | |
| `Rel` | `string?` | `null` | |
| `Current` | `bool` | `false` | Renders `<span aria-current="page">`, suppresses trailing separator. |
| `ChildContent` | `RenderFragment?` | `null` | |

### `UiButton` — `As="button"`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `ChildContent` | `RenderFragment?` | `null` | |
| `StartContent` / `EndContent` | `RenderFragment?` | `null` | |
| `Type` | `ButtonType` | `Button` | Use `Submit` inside `EditForm`. |
| `Color` | `ThemeColor` | `Primary` | |
| `Size` | `Size` | `Medium` | |
| `Variant` | `ButtonVariant` | `Solid` | Appearance only; Solid preserves the existing filled treatment. |
| `IconOnly` | `bool` | `false` | Uses square control geometry; provide `aria-label` or `aria-labelledby`. |
| `Disabled` | `bool` | `false` | Suppresses `OnClick`. |
| `IsLoading` | `bool` | `false` | Caller-controlled busy state; keeps the original content and accessible name, disables native buttons, and suppresses `OnClick` while true. |
| `OnClick` | `EventCallback<MouseEventArgs>` | — | |

Link form: `<UiButton As="a" href="/path">Go</UiButton>` — `href` flows through `AdditionalAttributes`; `Variant="ButtonVariant.Link"` changes appearance without changing the element or navigation semantics. For disabled/loading anchors, the caller controls navigation and supplies the appropriate `href`, `aria-disabled`, and `tabindex` attributes; the component does not rewrite them.

Primary color selection is deferred; this refinement preserves the existing palette and adds no theme settings.

### `UiCard` — `As="section"`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Title` | `string?` | `null` | Used only when `HeaderContent` is null. |
| `HeaderContent` | `RenderFragment?` | `null` | Takes precedence over `Title`. |
| `FooterContent` | `RenderFragment?` | `null` | |
| `ChildContent` | `RenderFragment?` | `null` | |

### `UiCheckbox` — `UiInputBase<bool>`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `ChildContent` | `RenderFragment?` | `null` | Label text. |

Bind with `@bind-Value`. `TryParseValueFromString` throws `NotSupportedException` — never bind `CurrentValueAsString`.

### `UiDivider`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Vertical` | `bool` | `false` | `false` ⇒ `<hr>`; `true` ⇒ `<div role="separator" aria-orientation="vertical">`. `As` is ignored. |

### `UiDrawer` — pure-CSS (hidden checkbox), works under static SSR

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Disabled` | `bool` | `false` | |
| `ShowBackdrop` | `bool` | `true` | |
| `CloseOnEscape` | `bool` | `true` | **No-op** — never read. |
| `Side` | `Align` | `Start` | |
| `Title` | `string?` | `null` | |
| `TriggerContent` | `RenderFragment?` | `null` | Rendered as `<label for>`. |
| `ChildContent` | `RenderFragment?` | `null` | |

**Uncontrolled** — there is no `Open` / `OpenChanged`. `@bind-Open` compiles (it falls into `AdditionalAttributes`) but does nothing.

### `UiFileInput` — `UiInputBase<IReadOnlyList<IBrowserFile>>`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Label` | `string?` | `null` | |
| `Multiple` | `bool` | `false` | |
| `MaxFiles` | `int` | `10` | Applied only when `Multiple`, as `Math.Max(1, MaxFiles)`. |
| `FilesChanged` | `EventCallback<IReadOnlyList<IBrowserFile>>` | — | Fires after `CurrentValue` is set. |

`OnInitialized` sets the value to an empty list.

### `UiMenu` — wraps `UiPopup`

| Parameter | Type | Default |
|---|---|---|
| `TriggerContent` / `ChildContent` | `RenderFragment?` | `null` |
| `TriggerAs` | `string` | `"button"` |
| `TriggerClass` / `ContentClass` / `BackdropClass` | `string?` | `null` |
| `TriggerAttributes` / `ContentAttributes` / `BackdropAttributes` | `IReadOnlyDictionary<string, object>?` | `null` |
| `Disabled` | `bool` | `false` |
| `CloseOnOutsideClick` | `bool` | `true` |
| `CloseOnEscape` | `bool` | `true` |
| `ShowBackdrop` | `bool` | `false` |

Per-slot classes, attributes and `TriggerAs` are forwarded to `UiPopup`.

### `UiMenuItem` — renders `<a>` when `Href` set, otherwise `<button>`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Href` / `Target` / `Rel` | `string?` | `null` | `Disabled` nulls out `href`. |
| `Disabled` | `bool` | `false` | Suppresses `OnClick`. |
| `StartContent` / `EndContent` / `ChildContent` | `RenderFragment?` | `null` | |
| `OnClick` | `EventCallback<MouseEventArgs>` | — | |

### `UiNumbox<TValue>` — `where TValue : INumber<TValue>`

No own parameters. Surface = `UiInputFieldBase<TValue>` + `DebounceDelay` + `UiInputBase` + `InputBase`.

- Uses `type="number"`, default `inputmode` (`decimal` for float/double/decimal, else `numeric`) and default `step` (`any` for decimal types, `1` for integers). Caller attributes are retained; explicit `step` and `inputmode` take precedence.
- Parses with `NumberStyles.Float` (no thousands grouping), InvariantCulture first then CurrentCulture. Formats with InvariantCulture.
- `public static readonly bool IsFloatingPoint`.

### `UiPopup` — JS-driven (`wwwroot/js/uiPopup.js` reads the `data-*` attributes)

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `TriggerContent` / `ChildContent` | `RenderFragment?` | `null` | |
| `TriggerAs` | `string` | `"button"` | Applied to the trigger element. |
| `TriggerMode` | `string` | `"toggle"` | Controls toggle/open/none click behavior. |
| `TriggerAttributes` | `IReadOnlyDictionary<string, object>?` | `null` | |
| `Disabled` | `bool` | `false` | → `data-disabled` |
| `CloseOnOutsideClick` | `bool` | `true` | → `data-close-outside` |
| `CloseOnEscape` | `bool` | `true` | → `data-close-escape` |
| `ShowBackdrop` | `bool` | `false` | → `data-show-backdrop` |

`TriggerClass`, `ContentClass`, `BackdropClass`, `ContentAttributes` and `BackdropAttributes` are accepted and applied to the corresponding elements.

Slots emitted: `data-slot="popup-root" | "popup-trigger" | "popup-backdrop" | "popup-content"`.

### `UiRadioGroup<TValue>` — `UiInputBase<TValue>`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `ChildContent` | `RenderFragment?` | `null` | Holds `UiRadio<TValue>` children. |
| `Name` | `string?` | `null` | Form field name. Falls back to `NameAttributeValue`, then an auto `radio-{guid}`. |

Cascades itself (`IsFixed="true"`) to children.

### `UiRadio<TValue>` — plain `ComponentBase` (**no `Class` / `As`**)

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Value` | `TValue` | — | `[EditorRequired]` |
| `ValueKey` | `string?` | `null` | Required when `Value` is not string-convertible, else throws `InvalidOperationException`. |
| `Disabled` | `bool` | `false` | OR'd with the group's `Disabled`. |
| `ChildContent` | `RenderFragment?` | `null` | |

Throws `InvalidOperationException` in `OnInitialized` if not nested in a `UiRadioGroup<TValue>`.

### `UiSelect<TValue>` — `UiInputFieldBase<TValue>`, `[CascadingTypeParameter]`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `ChildContent` | `RenderFragment?` | `null` | Select options. |
| `ClearText` | `string` | `"Clear value"` | |
| `NoSuggestionsText` | `string` | `"No suggestions"` | |
| `Label` | `string?` | `null` | Visible above the trigger and used as the input accessible name. |
| `Size` | `Size` | `Medium` | Sets the trigger height to the shared small/medium/large control sizes. |
| `Description` | `string?` | `null` | Shown below the field and linked through `aria-describedby`. |

Plus `Placeholder`, `DebounceDelay`, `Disabled` / `ReadOnly` / `Required` / `Class`, `@bind-Value`.
Requires explicit `TValue`. Filtering and keyboard behaviour live in `wwwroot/js/uiSelect.js`.

### `UiSelectItem<TValue>` — plain `ComponentBase, IDisposable` (**no `Class` / `As`**)

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Value` | `TValue` | — | `[EditorRequired]` |
| `Text` | `string?` | `null` | Falls back to `Value?.ToString()`. |
| `Disabled` | `bool` | `false` | OR'd with parent's `Disabled` / `ReadOnly`. |
| `Template` | `RenderFragment<TValue>?` | `null` | Highest precedence for item content. |
| `ChildContent` | `RenderFragment?` | `null` | Used when `Template` is null. |

Content precedence: `Template(Value)` → `ChildContent` → `Text` / `Value.ToString()`. Must be inside a `UiSelect<TValue>`.

### `UiSkeleton` — `As="div"`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `IsLoading` | `bool` | `false` | `[EditorRequired]`. Drives `data-loading` / `aria-busy`. |
| `ChildContent` | `RenderFragment?` | `null` | Real content. When loading with no `SkeletonContent`, it is wrapped in the auto-mimic class. |
| `SkeletonContent` | `RenderFragment?` | `null` | Custom placeholder shown instead while loading. |

### `UiSkeletonLine` / `UiSkeletonBlock` — `As="div"`

No own parameters — `Class` + `AdditionalAttributes` only. Render `aria-hidden="true"` placeholders.

### `UiSwitch` — `UiInputBase<bool>`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `ChildContent` | `RenderFragment?` | `null` | Label text. |

Same binding rule as `UiCheckbox`.

### `UiTextbox` — `UiInputFieldBase<string>`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Type` | `InputType` | `Text` | Applied on parameter updates, including changes after first render. |

### `UiTooltip` — `As="span"`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Text` | `string?` | `null` | Bubble text. |
| `Align` | `Align` | `Center` | |
| `Bottom` | `bool` | `false` | Places the bubble below instead of above. |
| `ChildContent` | `RenderFragment?` | `null` | The hover target. |

Tooltip text wraps, including unbroken strings, within a 14rem maximum width constrained by the viewport. CSS provides hover and keyboard-focus visibility; the JS initializer enhances horizontal placement to keep an 8px viewport gutter. `Align` remains the preferred placement, adjusted near viewport edges; `Bottom` is unchanged.

### `UiToastHost` — `UiComponentBase, IAsyncDisposable`

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Placement` | `ToastPlacement` | `TopRight` | |
| `MaxVisible` | `int` | `4` | Clamped to `>= 1`; oldest evicted on overflow. |
| `DangerStickyByDefault` | `bool` | `true` | `Danger` toasts with no explicit duration become sticky. |
| `NewestOnTop` | `bool` | `true` | Insert at index 0 vs append. |

Public methods: `Show(ToastOptions)` (throws `ArgumentNullException` on null), `Clear()`, `DisposeAsync()`.
Duration rules: explicit `DurationMs` clamped to `>= 0`, where `0` = sticky; else Danger + `DangerStickyByDefault` ⇒ sticky; else **4000 ms**. `ThemeColor.Inherit` normalizes to `Primary`.

### `UiToast` — the individual toast; normally you don't place it yourself

| Parameter | Type | Default |
|---|---|---|
| `Id` | `int` | `0` — `[EditorRequired]` |
| `Options` | `ToastOptions` | `ToastOptions.Default` — `[EditorRequired]` |
| `Sticky` | `bool` | `false` |
| `OnDismiss` | `EventCallback` | — |

### `DialogHost`

No parameters of its own (inherited ones are accepted but ignored — it renders no root element). Place once in the layout with an interactive render mode. Also exposes the same two `ShowAsync` methods as `IDialogService`.

---

## 5. Dialogs

```csharp
public interface IDialogService
{
    Task<DialogResult<TResult>> ShowAsync<TDialog, TResult>(
        DialogOptions options,
        IReadOnlyDictionary<string, object>? parameters,
        CancellationToken ct = default)
        where TDialog : BaseDialog<TResult>;

    Task ShowAsync<TDialog>(                       // no result
        DialogOptions options,
        IReadOnlyDictionary<string, object>? parameters,
        CancellationToken ct = default)
        where TDialog : BaseDialog;

    void RegisterHost(DialogHost host);
}
```

`options` and `parameters` are **required positional arguments** — no overload omits them; pass `null` for `parameters` when the dialog takes none.

| Type | Members |
|---|---|
| `DialogOptions` | `CloseOnEscape` = `true`, `DismissOnOverlayClick` = `true`, `Classes` = `""` (extra classes on the `<dialog>`), `static Default` |
| `DialogResult<TResult>` | `IsCancelled` (`[MemberNotNullWhen(false, nameof(Value))]`), `Value`, `static FromValue(TResult)`, `static Canceled` |
| `BaseDialog` | `protected IJSRuntime JsRuntime`, `[CascadingParameter] protected IDialogInstance DialogInstance`, `protected void Close()` |
| `BaseDialog<TResult>` | adds `protected IDialogInstance<TResult> DialogInstance`, `protected void SetResult(TResult)` |

Rendered as a native `<dialog>` opened with `showModal()`; body scroll is locked while open. Escape / overlay click / native close all yield `DialogResult<T>.Canceled`.

**Define a dialog:**

```razor
@inherits BaseDialog<bool>

<h1>Delete item?</h1>

<UiButton Color="ThemeColor.Danger" OnClick="@(() => SetResult(true))">Delete</UiButton>
<UiButton Color="ThemeColor.Secondary" OnClick="Close">Cancel</UiButton>
```

Dialog parameters are ordinary `[Parameter]` properties, supplied through the `parameters` dictionary.

**Show it and read the result:**

```razor
@rendermode InteractiveServer
@inject IDialogService DialogService

<UiButton OnClick="Confirm">Delete</UiButton>

@code {
    private async Task Confirm()
    {
        var result = await DialogService.ShowAsync<ConfirmDialog, bool>(
            new DialogOptions { DismissOnOverlayClick = true, CloseOnEscape = true, Classes = "md:max-w-md" },
            new Dictionary<string, object> { ["ItemName"] = "report.pdf" });

        if (!result.IsCancelled && result.Value)
        {
            // confirmed
        }
    }
}
```

---

## 6. Toasts

```csharp
public interface IToastService
{
    void Show(ToastOptions options);
    void Clear();
    void RegisterHost(UiToastHost host);
    void UnregisterHost(UiToastHost host);
}
```

`ToastOptions` (all `init`): `Color` = `Primary`, `Title` = `null`, `Content` = `null` (`RenderFragment`), `DurationMs` = `null`, `Class` = `null`; `static Default`.

There is **no plain-string message API** — the minimum body is a `RenderFragment` lambda:

```razor
@inject IToastService ToastService

@code {
    private void Notify() =>
        ToastService.Show(new ToastOptions
        {
            Color = ThemeColor.Success,
            Title = "Saved",
            Content = builder => builder.AddContent(0, "Changes were saved."),
        });

    private void Sticky() =>
        ToastService.Show(new ToastOptions
        {
            Color = ThemeColor.Info,
            Title = "Sync running",
            DurationMs = 0,                 // 0 = never auto-dismiss
            Content = builder => builder.AddContent(0, "Working…"),
        });

    private void ClearAll() => ToastService.Clear();
}
```

---

## 7. Usage snippets

### Buttons

```razor
<div class="flex flex-row flex-wrap gap-1">
    <UiButton OnClick="Save">Primary</UiButton>
    <UiButton Color="ThemeColor.Danger">Danger</UiButton>
    <UiButton Color="ThemeColor.Success" Size="Size.Large">Success</UiButton>
    <UiButton Disabled="true">Disabled</UiButton>
    <UiButton As="a" href="/simple-form">Link</UiButton>
</div>
```

### Form — Textbox / Numbox / Select / Checkbox

```razor
<EditForm Model="@FormData" FormName="one" OnValidSubmit="@HandleValidSubmit">
    <DataAnnotationsValidator />
    <ValidationSummary />

    <div class="flex flex-col gap-2">
        <UiTextbox Label="Name:" Placeholder="Some text" @bind-Value="FormData!.Name" />

        <UiNumbox Label="Age:" @bind-Value="FormData.Age" />

        <UiSelect TValue="string" Label="Country:" Placeholder="Select country" @bind-Value="FormData.Country">
            <UiSelectItem Value="@("Kazakhstan")" Text="Kazakhstan" />
            <UiSelectItem Value="@("Kyrgyzstan")" Text="Kyrgyzstan" />
            <UiSelectItem Value="@("Korea")" Text="Korea" />
        </UiSelect>

        <UiCheckbox @bind-Value="FormData.Agree">Agree</UiCheckbox>

        <UiButton Type="ButtonType.Submit">Submit</UiButton>
    </div>
</EditForm>
```

`UiSelect` needs an explicit `TValue`; string literals in `Value` need `@("...")`.

### Switch / RadioGroup / FileInput (works under static SSR — no `@rendermode`)

```razor
<EditForm Model="@Model" FormName="two" OnValidSubmit="OnValidSubmit" class="max-w-xl flex flex-col gap-3">
    <DataAnnotationsValidator />

    <UiSwitch @bind-Value="Model.NotificationsEnabled">Enable notifications</UiSwitch>

    <UiRadioGroup TValue="string" Name="plan" @bind-Value="Model.Plan">
        <UiRadio Value="@("free")">Free plan</UiRadio>
        <UiRadio Value="@("pro")">Pro plan</UiRadio>
        <UiRadio Value="@("enterprise")">Enterprise plan</UiRadio>
    </UiRadioGroup>

    <UiFileInput Label="Attachments" Multiple="true" @bind-Value="Model.Files" />

    <UiButton Type="ButtonType.Submit">Submit</UiButton>
</EditForm>
```

### Breadcrumb / Menu / Drawer

```razor
<UiBreadcrumb>
    <UiBreadcrumbItem Href="/">Home</UiBreadcrumbItem>
    <UiBreadcrumbItem Href="/navigation">Library</UiBreadcrumbItem>
    <UiBreadcrumbItem Current="true">Navigation</UiBreadcrumbItem>
</UiBreadcrumb>

<UiMenu>
    <TriggerContent>
        <span>Open Menu</span>
    </TriggerContent>
    <ChildContent>
        <UiMenuItem OnClick="OnMenuClick">Profile</UiMenuItem>
        <UiMenuItem Href="/">Go Home</UiMenuItem>
    </ChildContent>
</UiMenu>

<UiDrawer Title="Drawer Navigation">
    <TriggerContent>
        <span>Open Drawer</span>
    </TriggerContent>
    <ChildContent>
        <UiButton As="a" href="/" Color="ThemeColor.Secondary">Home</UiButton>
    </ChildContent>
</UiDrawer>
```

### Card / Divider / Avatar / Tooltip / Alert

```razor
<UiCard Title="Profile">
    <ChildContent>
        <div class="flex items-center gap-3">
            <UiAvatar Name="Alex Johnson" />
            <div class="text-body-sm text-neutral-60">alex@example.com</div>
        </div>
    </ChildContent>
    <FooterContent>
        <UiButton Color="ThemeColor.Secondary">View details</UiButton>
    </FooterContent>
</UiCard>

<div class="flex items-center gap-3">
    <span>Left</span>
    <UiDivider Vertical="true" class="h-6" />
    <span>Right</span>
</div>

<UiTooltip Text="Alex Johnson">
    <UiAvatar Name="Alex Johnson" Size="Size.Large" />
</UiTooltip>

<UiAlert Title="Connection restored"
         Color="ThemeColor.Success"
         Dismissible="true"
         @bind-Visible="_showSuccessAlert">
    Your session is healthy and everything is synced.
</UiAlert>
```

### Skeleton

Auto-mimic (wrap the real content):

```razor
<UiSkeleton IsLoading="@_isLoading">
    <div class="flex flex-col gap-2">
        <UiTextbox Label="Name" Placeholder="Enter name" @bind-Value="_name" />
        <UiButton>Save</UiButton>
    </div>
</UiSkeleton>
```

Custom placeholder:

```razor
<UiSkeleton IsLoading="@_isLoading">
    <SkeletonContent>
        <div class="flex flex-col gap-2">
            <UiSkeletonLine Class="h-4 w-28" />
            <UiSkeletonBlock Class="h-10 rounded-md" />
        </div>
    </SkeletonContent>
    <ChildContent>
        <div class="rounded-md border border-border bg-surface1 p-3 text-body text-foreground">Loaded: <strong>@Summary</strong></div>
    </ChildContent>
</UiSkeleton>
```

---

## 8. Theme tokens

Two layers, and the split is the whole point.

**Roles** are the vocabulary — utility classes named for what a color *means*. This is what you write.
**Primitives** (`--graphite-*` custom properties) are the palette — the raw values roles resolve to. This is what you *override*. They are not exposed as utility classes.

Because nothing above the primitive layer holds a literal, changing any color is a CSS edit. No component class string names a value or a ramp step, so a retune never needs a library rebuild. Light values live at `:root` in `GraphiteUi/Styles/_theme.css`; `_dark.css` overrides them under `:root.dark, :root[data-theme="dark"]`.

### 8.1 Roles — use these

| Group | Tailwind class stems |
|---|---|
| Page | `background`, `foreground` |
| Surfaces | `surface1`, `surface2`, `surface3` (+ `-foreground` each), `divider`, `scrim` |
| Surface states | `surface-hover`, `surface-active`, `disabled-surface`, `selected`, `selected-foreground`, `skeleton` |
| Content | `foreground` (body), `muted-foreground` (secondary), `subtle-foreground` (tertiary), `disabled-foreground` |
| Validation | `error-foreground` (text), `invalid` (border), `focus-invalid` (ring) |
| Borders | `border`, `border-strong`, `control`, `control-hover`, `disabled`, `focus`, `invalid` — all as `border-*` |
| Focus | `focus` (ring + accent-tinted text), `focus-invalid` |
| Controls | `control-track`, `control-track-hover`, `control-thumb`, `control-thumb-disabled`, `control-checked`, `control-checked-hover`, `control-checked-foreground` |
| Accent (`ThemeColor.Primary`) | `accent`, `accent-hover`, `accent-active`, `accent-foreground`, `accent-subtle`, `accent-subtle-foreground`, `accent-border` |
| Secondary (`ThemeColor.Secondary`) | `secondary`, `secondary-hover`, `secondary-active`, `secondary-foreground`, `secondary-border`, `secondary-border-hover`, `secondary-subtle`, `secondary-subtle-foreground`, `secondary-border-subtle`, `secondary-indicator` |
| Success / Warning / Danger / Info | `<name>`, `<name>-hover`, `<name>-active`, `<name>-foreground`, `<name>-subtle`, `<name>-subtle-foreground`, `<name>-border` |
| Shimmer | `shimmer-from`, `shimmer-via` |
| Text scale | `text-h1`…`text-h6`, `text-body-lg`, `text-body`, `text-body-sm`, `text-caption`, `text-heading` |
| Radius | `rounded-sm/md/lg/xl` — controls `md`, containers `lg`, dialog `xl` |
| Shadows | `shadow-xs/sm/md/lg/xl/2xl` |

Usage: `class="text-body-sm text-muted-foreground bg-surface1"`.

Border roles read as `border-border`, `border-border-strong`, `border-control`, `border-control-hover`, `border-disabled`, `border-focus`, `border-invalid`. The doubled word in `border-border` is Tailwind's utility prefix plus the role name; it is the default container hairline.

> **Adding a role?** Tailwind builds `bg-X`, `text-X`, `border-X` and `ring-X` from a single `--color-X`. The token name is the part *after* the prefix, so a border role must be `--color-control`, not `--color-border-control` — the latter compiles to `border-border-control` and leaves `border-control` silently undefined. One token serves every property: `--color-focus` backs both `ring-focus` and `border-focus`.

### 8.2 Primitives — override these

Never referenced as classes. Redefine them to retheme.

| Group | Custom properties |
|---|---|
| Page / surfaces | `--graphite-background`, `--graphite-foreground`, `--graphite-surface1/2/3` (+ `-foreground`) |
| Neutral overlay ramp | `--graphite-neutral-5/10/15/20/30/50/60/70` |
| Inverse overlay ramp | `--graphite-overlay-5/10/15/20/30/50/90` |
| Accent ramp | `--graphite-accent-50` … `--graphite-accent-950`, plus `--graphite-accent`, `-hover`, `-active`, `-foreground`, `-subtle`, `-subtle-foreground`, `-border` |
| Lines | `--graphite-divider`, `--graphite-control-border`, `--graphite-scrim` |
| Semantic fills | `--graphite-success/warning/danger/info` (+ `-foreground`, `-subtle`, `-subtle-foreground`, `-border`, `-hover`, `-active`) |
| Focus, shimmer, shadows | `--graphite-focus`, `--graphite-shimmer-from/via`, `--graphite-shadow-xs`…`-2xl` |

### Control sizes and popup motion

These inherited CSS properties can be overridden after the stylesheet import or on a containing element. They remain variable references in generated CSS and do not require rebuilding GraphiteUi.

| Property | Default | Applies to |
|---|---|---|
| `--control-height-sm` | `calc(var(--spacing, 0.25rem) * 8)` | Small buttons and input wrappers |
| `--control-height-md` | `calc(var(--spacing, 0.25rem) * 10)` | Medium buttons/input wrappers and popup, drawer, menu, and select triggers |
| `--control-height-lg` | `calc(var(--spacing, 0.25rem) * 12)` | Large buttons and input wrappers |
| `--popup-backdrop-duration` | `220ms` | Popup backdrop open/close transitions |
| `--popup-content-duration` | `240ms` | Popup/menu content open/close transitions |
| `--popup-easing` | `cubic-bezier(0.16, 1, 0.3, 1)` | Popup backdrop/content easing |

At the standard spacing and 16px root font size, control heights are 32/40/48px. Input wrappers use the same property for height and minimum height. Option rows and unrelated component animations retain their own sizing and motion. Reduced-motion preferences still disable popup transitions.

```css
.compact-form {
    --control-height-md: 2.25rem;
    --popup-backdrop-duration: 120ms;
    --popup-content-duration: 140ms;
    --popup-easing: ease-out;
}
```

Component root and part classes are passed through unchanged. Consumer utility classes can override library appearance because GraphiteUi component selectors use the `components` layer with low specificity.

### Style ownership and normalization

Component roots expose stable `data-ui` identities, and their styled children expose `data-part` markers. For example, Menu keeps `data-ui="popup"` and adds `data-menu="true"`; JS continues to use the existing `data-slot` and state attributes. Enum state attributes use normalized lowercase kebab-case values, and boolean state attributes use `true` or `false`.

The required library stylesheet entrypoint is `GraphiteUi/Styles/graphite-ui.css`. It does not import Tailwind itself; import it after `tailwindcss`. It imports per-family styles from the same `Styles/` directory and scopes base normalization to library roots and their owned parts.

### 8.3 Rebranding without touching the library

`@theme static inline` inlines the primitive reference into every generated utility — `bg-accent` compiles to `background-color: var(--graphite-accent)`, not to a hex. So redefining a primitive at `:root` **after** the theme import retints the whole library at runtime, with no rebuild of GraphiteUi:

```css
@import "tailwindcss";
@import "../../bin/GraphiteUi/Styles/graphite-ui.css";

/* Swap the graphite accent for a real hue. */
:root {
    --graphite-accent-900: #1D4ED8;
    --graphite-accent-800: #1E40AF;
    --graphite-accent-700: #1E3A8A;
}
```

That single block moves primary buttons, checked checkboxes/radios/switches, focus rings, the focused input border, and the selected select row — because every one of them goes through a role that resolves to the accent.

To retune a *role* rather than a value — "disabled text should be lighter", "the switch track needs another step" — rebind the role instead:

```css
@theme static inline {
    --color-disabled-foreground: var(--graphite-neutral-20);
    --color-control-track: var(--graphite-neutral-15);
}
```

### Rules worth knowing

**The neutral ramp flips polarity per theme.** `--graphite-neutral-*` is black-alpha in light and white-alpha in dark; `--graphite-overlay-*` is the reverse. The number is the alpha percent. That is why roles built on it — `surface-hover`, `muted-foreground`, `control-track` — are correct in both themes with no `dark:` variant, and it is what makes the light theme cheap.

**Hover and active are roles, not shades.** Use `hover:bg-accent-hover`, never a numbered step. Semantic fills move *away* from their own foreground on hover, so contrast improves in every state. The accent is the exception — it already sits at an extreme of the ramp, so it steps one notch toward the middle (17.72:1 base, 14.89:1 hover, 10.44:1 active in light).

**The accent is a lightness inversion, not a hue.** `ThemeColor.Primary` is a near-black fill with white text in light (`--graphite-accent-900`) and a white fill with near-black text in dark (`--graphite-accent-50`). Emphasis comes from inverting against the page. `ThemeColor.Secondary` is the graphite fill, and is the only solid tone that draws a visible border — its fill sits ~1.2–1.4:1 from the page, so the edge is what gives it a shape.

**Two border weights, deliberately.** `border-control` holds 3:1 (WCAG 1.4.11) for anything interactive — inputs, checkboxes, radios, switches, triggers. `border-border` is a decorative hairline for containers and measures ~1.2:1 against the page; do not use it as a control boundary.

### Renamed in this version

The numbered ramps are no longer utility classes. Every step now reaches you through a role.

| Old | New |
|---|---|
| `text-neutral-60`, `placeholder:text-neutral-60` | `text-muted-foreground`, `placeholder:text-muted-foreground` |
| `text-neutral-50` | `text-subtle-foreground` |
| `text-neutral-30`, `text-neutral-20` | `text-disabled-foreground` |
| `bg-neutral-5` | `bg-disabled-surface` |
| `hover:bg-neutral-10` | `hover:bg-surface-hover` |
| `hover:bg-neutral-15` | `hover:bg-surface-active` |
| `bg-neutral-10` (skeleton / switch track) | `bg-skeleton` / `bg-control-track` |
| `bg-neutral-60`, `bg-neutral-30` (switch thumb) | `bg-control-thumb`, `bg-control-thumb-disabled` |
| `bg-neutral-50` (alert indicator) | `bg-secondary-indicator` |
| `border-neutral-10`, `border-neutral-20` | `border-border`, `border-border-strong` |
| `border-control-border` | `border-control` |
| `hover:border-foreground` | `hover:border-control-hover` |
| `border-danger` on a control, `ring-danger` | `border-invalid`, `ring-focus-invalid` |
| `bg-accent` / `border-accent` on a control | `bg-control-checked` / `border-control-checked` |
| `bg-accent-subtle` on a selected row | `bg-selected` / `text-selected-foreground` |
| `accent-50`…`accent-950`, `neutral-*`, `overlay-*` | removed — primitives only |
| `primary`, `primary-5`…`primary-70` | `neutral` primitives (previous version) |
| `secondary`, `secondary-5`…`secondary-90` | `overlay` primitives (previous version); `secondary-*` is now the Secondary tone |
| `text-regular`, `text-medium` | `text-body` |
| `text-small` | `text-body-sm` |
| `text-large`, `text-header` | `text-body-lg` / `text-h5` |

The old `text-*` size names still resolve as deprecated aliases. The **color** names do not.

> **Watch out for `neutral-50`.** Tailwind v4 ships its own `neutral-50`…`neutral-950` palette. Now that GraphiteUi no longer defines `--color-neutral-*`, a stale `text-neutral-50` silently resolves to Tailwind's near-white `#fafafa` instead of failing. Every other removed name (`neutral-5/10/15/20/30/60/70`, `overlay-*`, `accent-<n>`) has no Tailwind counterpart and simply stops compiling.

---

## 9. Known gaps

Parameters that compile but do nothing, and behaviours that will surprise you. Do not write code that depends on them.

| Item | Detail |
|---|---|
| `UiInputFieldBase.Class` | Textbox/Numbox apply `Class` raw to their outer wrapper. `UiSelect` passes it unchanged to its existing Popup root. |
| `UiDrawer.CloseOnEscape` | Never read — the drawer is pure CSS with no JS or data attribute. |
| `UiDrawer` open state | No `Open` / `OpenChanged` parameter. `@bind-Open` compiles (falls into `AdditionalAttributes`) and silently does nothing. |
| `UiSelect.UnregisterItem` | Empty body, so `UiSelectItem.Dispose()` is a no-op. |
| Namespace casing | `AddGraphiteUi` lives in `GraphiteUI.Extensions` (capital `UI`). `GraphiteUi.Extensions` (lowercase) is a *different*, real namespace of internal helpers — C# namespaces are case-sensitive, so the wrong one fails to resolve. |
| No XML docs shipped | `GenerateDocumentationFile` is commented out in `GraphiteUi.csproj`, so the `///` comments never reach IntelliSense. This file is the reference. |

### Fixed in the design-system pass

Recorded so the same ground is not re-audited: five components (`UiSwitch`, `UiRadio`, `UiPopup`, `UiSelect`, `UiDrawer`) rendered **no disabled state**, because `opacity-disabled` was never a real utility and compiled to nothing — it is now declared via `@utility`. `reduce-motion:` was the wrong prefix for `motion-reduce:`. `surface1` composited to exactly `surface2`'s value, so cards and popups were the same color. The focus ring was 6px of 5%-white (~1.1:1) and buttons drew it twice, once on `focus` rather than `focus-visible`. `-400` was darker than the base for success/warning but lighter for danger/info, so `hover:` changed direction per color. Placeholder colour sat on the wrapper `div`, where the `placeholder:` variant matched nothing, so Tailwind's preflight default applied (3.35:1 on light). The light theme did not exist. `--color-primary-1` aliased an undefined primitive, and `--shadow-xl` pointed at the `lg` primitive. `UiToast` passed a non-existent `AutoHideOnDismiss` parameter to `UiAlert`.

See [ParcelStorage integration](parcelstorage.md) for the adapted development workflow and validation.

using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components;

internal abstract class DialogInstanceBase<TDialog> : IDialogInstance
    where TDialog : BaseDialog
{
    protected DialogInstanceBase(DialogId id, DialogOptions options, IReadOnlyDictionary<string, object>? parameters)
    {
        Id = id;
        Options = options;
        _zIndexStyle = $"z-index: {Id.Value + 100}";
        Render = BuildRender(parameters);
    }

    public DialogId Id { get; }
    public DialogOptions Options { get; }
    public RenderFragment Render { get; }
    public ElementReference DialogElementRef { get; private set; }

    private readonly string _zIndexStyle;

    public abstract Task WaitAsync(CancellationToken ct);
    public abstract void Close();


    private RenderFragment BuildRender(IReadOnlyDictionary<string, object>? parameters) => builder =>
    {
        var seq = 0;

        builder.OpenComponent<CascadingValue<IDialogInstance>>(seq++);
        builder.AddComponentParameter(seq++, nameof(CascadingValue<IDialogInstance>.IsFixed), true);
        builder.AddComponentParameter(seq++, nameof(CascadingValue<IDialogInstance>.Value), this);
        builder.AddComponentParameter(seq++, nameof(CascadingValue<IDialogInstance>.ChildContent), CreateDialogMarkup(parameters));
        builder.CloseComponent();
    };

    private RenderFragment CreateDialogMarkup(IReadOnlyDictionary<string, object>? parameters) => builder =>
    {
        int seq = 0;

        builder.OpenElement(seq++, "dialog");
        builder.AddAttribute(seq++, "class", string.IsNullOrWhiteSpace(Options.Classes)
            ? DialogStyles.Classes
            : $"{DialogStyles.Classes} {Options.Classes}");
        builder.AddAttribute(seq++, "style", _zIndexStyle);
        builder.AddAttribute(seq++, "id", Id.Value);
        builder.AddAttribute(seq++, "role", "dialog");
        builder.AddAttribute(seq++, "data-slot", "dialog");
        builder.AddAttribute(seq++, "aria-modal", "true");

        builder.AddAttribute(seq++, "onclose", EventCallback.Factory.Create<EventArgs>(this, Close));

        builder.AddElementReferenceCapture(seq++, reference => DialogElementRef = reference);
        builder.AddContent(seq++, CreateDialogComponent(parameters));
        builder.CloseElement();
    };

    private static RenderFragment CreateDialogComponent(IReadOnlyDictionary<string, object>? parameters) => builder =>
    {
        var seq = 0;
        builder.OpenComponent<TDialog>(seq++);

        if (parameters is not null)
        {
            foreach (var (name, value) in parameters)
            {
                builder.AddComponentParameter(seq++, name, value);
            }
        }

        builder.CloseComponent();
    };
}

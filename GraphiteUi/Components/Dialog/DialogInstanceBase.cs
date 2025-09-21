using GraphiteUi.Styles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GraphiteUi.Components;

internal abstract class DialogInstanceBase<TDialog> : IDialogInstance
    where TDialog : BaseDialog
{
    protected DialogInstanceBase(DialogId id, DialogOptions options, IReadOnlyDictionary<string, object>? parameters)
    {
        Id = id;
        Options = options;
        Render = BuildRender(parameters);
    }

    public DialogId Id { get; }
    public DialogOptions Options { get; }
    public RenderFragment Render { get; }
    public ElementReference DialogElementRef { get; private set; }

    public abstract Task WaitAsync(CancellationToken ct);
    public abstract void Close();


    public RenderFragment BuildRender(IReadOnlyDictionary<string, object>? parameters) => builder =>
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
        builder.AddAttribute(seq++, "class", $"{DialogStyles.Classes} {Options.Classes}");
        builder.AddAttribute(seq++, "style", $"z-index: {Id.Value + 100}");
        builder.AddAttribute(seq++, "id", Id.Value);
        builder.AddAttribute(seq++, "role", "dialog");
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
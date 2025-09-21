using Microsoft.AspNetCore.Components;

namespace GraphiteUi.Components.Dialog;

[EventHandler("onclose", typeof(EventArgs), enableStopPropagation: true, enablePreventDefault: false)]
[EventHandler("oncancel", typeof(EventArgs), enableStopPropagation: true, enablePreventDefault: true)]
public static class DialogEventHandlers { }
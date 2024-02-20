using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TomsApp.Components;
public partial class WarningDialog
{
    [CascadingParameter] 
    MudDialogInstance MudDialog { get; set; }

    [Parameter] 
    public Action? ConfirmedAction { get; set; }

    void Submit() => MudDialog.Close(DialogResult.Ok(true));
    void Cancel() => MudDialog.Cancel();

}

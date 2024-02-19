using Microsoft.AspNetCore.Components;
using MudBlazor;
using TomsApp.Models;

namespace TomsApp.Components.Pages;
public partial class StatsAndSkills
{
    [Inject]
    private ISnackbar _snackbar { get; set; } = default!;

    [Parameter]
    [EditorRequired]
    public Stat? Stat { get; set; }

    private string? newSkill;

    /*async*/
    private void AddSkill(List<Skill> Skills)
    {
        if (!string.IsNullOrWhiteSpace(newSkill))
        {
            Skills.Add(new Skill { Name = newSkill });
            newSkill = string.Empty;
            _snackbar.Add("Skill added", Severity.Info);
            //await Save();
        }
    }
}

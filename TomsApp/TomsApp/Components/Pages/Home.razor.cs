using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TomsApp.Models;

namespace TomsApp.Components.Pages;
public partial class Home
{
    private const string CHARACTER = "Character";


    [Inject]
    private ILocalStorageService _localStorageService { get; set; } = default!;

    [Inject]
    private ISnackbar _snackbar { get; set; } = default!;

    private Character _character = new();
    private string? _newSkill;

    private void AddSkill()
    {
        if (!string.IsNullOrWhiteSpace(_newSkill))
        {
            _character.Brawn.Skills.Add(new Skill { Name = _newSkill });
            _newSkill = string.Empty;
            _snackbar.Add("Skill added", Severity.Info);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (!firstRender)
            return;
        await ReadData();
        StateHasChanged();
    }

    private async Task ReadData() {
        try {
            _character = await _localStorageService.GetItemAsync<Character>(CHARACTER) ?? new();
        } catch (Exception) {
            await _localStorageService.RemoveItemsAsync(new string[] { CHARACTER });
            _character = await _localStorageService.GetItemAsync<Character>(CHARACTER)
                ?? new();
        }
    }

    private async Task Save() {
        await _localStorageService.SetItemAsync(CHARACTER, _character);
        _snackbar.Add("Saved Successfully", Severity.Info);
    }

    private void ChangeValue(string value)
    {
        _character.Name = value;
    }
}

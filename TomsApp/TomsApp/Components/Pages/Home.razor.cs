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
    private string? newSkill;
    private string? newWeapon;
    private string? newOther;

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

	private void AddWeapon(List<Weapon> Weapons)
	{
		if (!string.IsNullOrWhiteSpace(newWeapon))
		{
			Weapons.Add(new Weapon { Name = newWeapon });
			newWeapon = string.Empty;
			_snackbar.Add("Weapon added", Severity.Info);
			//await Save();
		}
	}

	private void AddOther(List<string> Others)
	{
		if (!string.IsNullOrWhiteSpace(newOther))
		{
			Others.Add(newOther);
			newOther = string.Empty;
			_snackbar.Add("Added", Severity.Info);
			//await Save();
		}
	}

}

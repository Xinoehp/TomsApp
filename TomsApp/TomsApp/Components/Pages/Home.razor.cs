using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;
using TomsApp.Models;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Forms;
using TomsApp.Services;


namespace TomsApp.Components.Pages;
public partial class Home
{
	private const string CHARACTER = "Character";

	[Inject]
	private IJSRuntime _js { get; set; } = default!;

	[Inject]
	private ILocalStorageService _localStorageService { get; set; } = default!;

	[Inject]
	private ISnackbar _snackbar { get; set; } = default!;

	[Inject]
	private IDialogService _dialogService { get; set; } = default!;

	[Inject]
	private CharacterService _characterService { get; set; } = default!;

	//private Character _character = new();
	private string? _newWeapon;
	private string? _newOther;



	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
			return;
		await ReadData();
		StateHasChanged();
	}

	private async Task ReadData()
	{
		try
		{
			_characterService.Current = await _localStorageService.GetItemAsync<Character>(CHARACTER) ?? new();
		}
		catch (Exception)
		{
			await _localStorageService.RemoveItemsAsync(new string[] { CHARACTER });
			_characterService.Current = await _localStorageService.GetItemAsync<Character>(CHARACTER)
				?? new();
		}
	}

	private async Task Save()
	{
		await _localStorageService.SetItemAsync(CHARACTER, _characterService.Current);
		_snackbar.Add("Saved Successfully", Severity.Success);
	}

	private void AddWeapon(List<Weapon> Weapons)
	{
		if (!string.IsNullOrWhiteSpace(_newWeapon))
		{
			Weapons.Add(new Weapon { Name = _newWeapon });
			_newWeapon = string.Empty;
			_snackbar.Add("Weapon added", Severity.Success);
			//await Save();
		}
	}

	private void AddOther(List<string> Others)
	{
		if (!string.IsNullOrWhiteSpace(_newOther))
		{
			Others.Add(_newOther);
			_newOther = string.Empty;
			_snackbar.Add("Added", Severity.Success);
			//await Save();
		}
	}

	private async Task ClearCharacterSelected()
	{
        var options = new DialogOptions { CloseOnEscapeKey = true };
        var result = await (await _dialogService.ShowAsync<WarningDialog>("Warning, this will delete your character!", options)).Result;
        if (!result.Canceled && (bool)(result.Data ?? false))
        {
			await ClearCharacter();
        }
    }

	private async Task ClearCharacter()
	{
		_characterService.Current = new Character();
		await Save();
	}


	
}

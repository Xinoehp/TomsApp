using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;
using System.Text;
using TomsApp.Models;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Mvc;
using static MudBlazor.CategoryTypes;
using Microsoft.AspNetCore.Components.Forms;


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

	private Character _character = new();
	private string? newSkill;
	private string? newWeapon;
	private string? newOther;
	private bool showClearDialog;
	IList<IBrowserFile> files = new List<IBrowserFile>();

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
			_character = await _localStorageService.GetItemAsync<Character>(CHARACTER) ?? new();
		}
		catch (Exception)
		{
			await _localStorageService.RemoveItemsAsync(new string[] { CHARACTER });
			_character = await _localStorageService.GetItemAsync<Character>(CHARACTER)
				?? new();
		}
	}

	private async Task Save()
	{
		await _localStorageService.SetItemAsync(CHARACTER, _character);
		_snackbar.Add("Saved Successfully", Severity.Info);
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

	private async Task ExportCharacterAsync()
	{
		var json = JsonSerializer.Serialize(_character);

		string fileName = $"{_character.Name}-{DateTime.Today:yyyy-MM-dd}";
		await _js.InvokeVoidAsync("downloadObjectAsJson", json, fileName);
	}

	private async Task ClearCharacterSelected()
	{
        var options = new DialogOptions { CloseOnEscapeKey = true };
        var result = await (await _dialogService.ShowAsync<WarningDialog>("Warning, this will delete your character!", options)).Result;
        if (!result.Canceled && (bool)(result.Data ?? false))
        {
			ClearCharacter();
        }
    }

	private async Task ClearCharacter()
	{
		_character = new Character();
		await Save();
	}



	private async Task UploadFiles(IBrowserFile file)
	{
		var stream = file.OpenReadStream();
		_character = await JsonSerializer.DeserializeAsync<Character>(stream);

		await _localStorageService.SetItemAsync(CHARACTER, _character);
		_snackbar.Add("Upload Successful", Severity.Info);

	}
}

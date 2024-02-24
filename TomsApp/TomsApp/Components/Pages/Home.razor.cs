using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;
using TomsApp.Models;
using Microsoft.JSInterop;
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
	private string? _newWeapon;
	private string? _newOther;

	private static readonly JsonSerializerOptions _jsonIgnoreCase = new()
	{
		PropertyNameCaseInsensitive = true,
	};



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

	private async Task ExportCharacterAsync()
	{

		string fileName = $"{_character.Name}-{DateTime.Today:yyyy-MM-dd}";
		await _js.InvokeVoidAsync("downloadObjectAsJson", _character, fileName);
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
		_character = new Character();
		await Save();
	}


	private async Task UploadFiles(IBrowserFile file)
	{
		using var stream = file.OpenReadStream();

		try
		{
			var character = await JsonSerializer.DeserializeAsync<Character>(stream, _jsonIgnoreCase);
			if (character == null)
			{
				_snackbar.Add("Could not load character!", Severity.Error);
				return;
			}
			_character = character;
			await _localStorageService.SetItemAsync(CHARACTER, _character);
			_snackbar.Add("Upload Successful", Severity.Success);
		}
		catch (Exception)
		{
			_snackbar.Add("Could not load character!", Severity.Error);
			throw;
		}

	}
}

using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;
using TomsApp.Models;
using TomsApp.Services;

namespace TomsApp.Components.Layout;
public partial class MainLayout
{
	private const string CHARACTER = "Character";
	private bool _isDarkMode = true;
	private bool _open = false;


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

	private static readonly JsonSerializerOptions _jsonIgnoreCase = new() {
		PropertyNameCaseInsensitive = true,
	};


	private void toggle()
	{
		_open = !_open;
	}

	private void darkModeToggle()
	{
		_isDarkMode = !_isDarkMode;
	}

	private async Task ClearCharacterSelected()
	{
		// run home.ClearCharacterSelected()?
	}

	private async Task UploadFiles(IBrowserFile file) {
		using var stream = file.OpenReadStream();

		try {
			var character = await JsonSerializer.DeserializeAsync<Character>(stream, _jsonIgnoreCase);
			if (character == null) {
				_snackbar.Add("Could not load character!", Severity.Error);
				return;
			}
			_characterService.Current = character;
			await _localStorageService.SetItemAsync(CHARACTER, _characterService.Current);
			_snackbar.Add("Upload Successful", Severity.Success);
		} catch (Exception) {
			_snackbar.Add("Could not load character!", Severity.Error);
			throw;
		}

	}

	private async Task ExportCharacterAsync() {

		string fileName = $"{_characterService.Current.Name}-{DateTime.Today:yyyy-MM-dd}";
		await _js.InvokeVoidAsync("downloadObjectAsJson", _characterService.Current, fileName);
	}
}

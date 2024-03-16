using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using System.Runtime.ConstrainedExecution;
using System;
using System.Text.Json;
using TomsApp.Components;
using TomsApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TomsApp.Services;
public class CharacterService
{
	private readonly ILocalStorageService _localStorageService;
	private readonly ISnackbar _snackbar;
	private readonly IDialogService _dialogService;
	private readonly IJSRuntime _js;

	public CharacterService(ILocalStorageService localStorageService, ISnackbar snackbar, IDialogService dialogService, IJSRuntime jSRuntime)
    {
        _localStorageService = localStorageService;
		_snackbar = snackbar;
		_dialogService = dialogService;
		_js = jSRuntime;
	}

    private const string CHARACTERS = "Characters";

	public event Action? Updated;

	public CharacterList CharacterList { get; set; } = new CharacterList();

    public Character Current { get; set; } = new Character();

	public async Task ReadData()
	{
		try
		{
			CharacterList = await _localStorageService.GetItemAsync<CharacterList>(CHARACTERS) ?? new();
			Updated?.Invoke();
		}
		catch (Exception)
		{
			await _localStorageService.RemoveItemsAsync(new string[] { CHARACTERS });
			CharacterList = new();
			Current = new() ;
		}
	}

	public async Task Save()
	{
		var characterPosition = CharacterList.Characters.FindIndex(c => c.Id == Current.Id);
		if (characterPosition == -1)
		{
			CharacterList.Characters.Add(Current);
			await UpdateStorage("Created New Character!");
			return;
		}
		CharacterList.Characters[characterPosition] = Current;
		await UpdateStorage("Saved Successfully!");
	}

	public async Task UpdateStorage(string? message)
	{
		await _localStorageService.SetItemAsync(CHARACTERS, CharacterList);
		if (message != null) _snackbar.Add(message, Severity.Success);
		Updated?.Invoke();
	}

	public async Task UpdateId(Character character)
	{
		CharacterList.ActiveCharacterId = character.Id;
		await UpdateStorage(null);
	}

	private async Task<bool> WarnUser(string warningMessage)
	{
		var options = new DialogOptions { CloseOnEscapeKey = true };
		var result = await (await _dialogService.ShowAsync<WarningDialog>(warningMessage, options)).Result;
		return !result.Canceled && (bool)(result.Data ?? false);
	}

	public async Task<bool> TrySaveCharacter()
	{
		var savedCharacters = await _localStorageService.GetItemAsync<CharacterList>(CHARACTERS) ?? new CharacterList();
		if (DirtyCheck(savedCharacters))
		{
			return await WarnUser("Warning, your unsaved changes will be lost.");
		}
		return true;
	}

	public async Task NewCharacter()
	{
		if (!await TrySaveCharacter()) return;
		await ReadData();
		CharacterList.Characters.Add(new Character());
		Current = CharacterList.Characters[CharacterList.Characters.Count - 1];
		CharacterList.ActiveCharacterId = Current.Id;
		Updated?.Invoke();
	}

	public async Task LoadCharacter(Character character)
	{
		if (!await TrySaveCharacter()) return;
		await ReadData();
		Current = character;
		await UpdateId(Current);
		_snackbar.Add("Loaded " + character.Name + ".", Severity.Success);
	}

	private bool DirtyCheck(CharacterList SavedCharacters)
	{
		int characterPosition = SavedCharacters.Characters.FindIndex(c => c.Id == Current.Id);
		if (characterPosition != -1)
		{
			string CurrentState = JsonSerializer.Serialize<Character>(SavedCharacters.Characters[characterPosition]);
			string SavedState = JsonSerializer.Serialize<Character>(Current);

			if (CurrentState == SavedState)
			{
				return false;
			}
		}
		return true;
	}

	public async Task DeleteCharacter(Character character)
	{
		var options = new DialogOptions { CloseOnEscapeKey = true };
		var result = await (await _dialogService.ShowAsync<WarningDialog>("Warning, " + character.Name + " will be deleted.", options)).Result;
		if (!result.Canceled && (bool)(result.Data ?? false))
		{
			CharacterList.Characters.Remove(character);
			if (Current == character)
			{
				Current = CharacterList.Characters.LastOrDefault() ?? new();
				await UpdateId(Current);
			}
			await UpdateStorage("Character Deleted!");
		}
	}

	public async Task ExportCharactersAsync()
	{
		string fileName = $"Characters-{DateTime.Today:yyyy-MM-dd}";
		await _js.InvokeVoidAsync("downloadObjectAsJson", CharacterList, fileName);
	}

	private static readonly JsonSerializerOptions _jsonIgnoreCase = new()
	{
		PropertyNameCaseInsensitive = true,
	};

	public async Task UploadCharacters(IBrowserFile file)
	{
		if (!await WarnUser("Uploading characters will clear your current characters!")) return;
		using var stream = file.OpenReadStream();

		try
		{
			var uploadedCharacters = await JsonSerializer.DeserializeAsync<CharacterList>(stream, _jsonIgnoreCase);
			if (uploadedCharacters == null)
			{
				_snackbar.Add("Could not load characters!", Severity.Error);
				return;
			}
			CharacterList = uploadedCharacters;
			if (CharacterList.ActiveCharacterId == Guid.Empty)
			{
				Current = CharacterList.Characters.LastOrDefault() ?? new();
				await UpdateId(Current);
			}
			else
			{
				Current = CharacterList.Characters.Find(c => c.Id == CharacterList.ActiveCharacterId);
			}
			await _localStorageService.SetItemAsync(CHARACTERS, CharacterList.Characters);
			await UpdateStorage("Upload Successful!");
		}
		catch (Exception)
		{
			_snackbar.Add("Could not load characters!", Severity.Error);
			throw;
		}

	}

	public Dictionary<string, string> rolesNew = new()
	{
		{ "Bio Hacker", "Once per game you may auto-pass a Bio Hacking Hacking or Bio Hacking Persuasion test." },
		{ "Core Hacker", "You may spend 1 Grit to reroll a 12 in a Core Hacking test. Only 1 reroll per skill test." },
		{ "CyberDoc", "Once per game you may auto-pass a installation or repair of cybernetics test." },
		{ "Data Dealer", "You may spend 2 Grit to auto-pass a Surveillance or Deceive skill test." },
		{ "Handler", "You may spend 1 Grit to auto-pass a Meld skill test. -1 modifier to all Awareness skill tests." },
		{ "Mech", "You may spend 1 Grit to reroll a 12 in driving, riding, piloting or vehicle repair tests. Only 1 reroll per skill test." },
		{ "MilTech", "-1 to all Electronics and Weapons Tech skill tests related to making repairs." },
		{ "Operator", "+1 to initiative at the start of combat." },
		{ "ParaMed", "Each time you give medical aid, the subject recovers 1 additional (E) and 1 point of Morale." },
		{ "Tech Trader", "-1 to all Streetwise and Barter skill tests related to locating items, buyers and negotiating prices." },
	};

	public async Task<IEnumerable<string>> RolesSearch(string value)
	{
		// In real life use an asynchronous function for fetching data from an api.
		await Task.Delay(5);

		// if text is null or empty, show complete list
		if (string.IsNullOrEmpty(value)) return rolesNew.Keys;
		return rolesNew.Keys.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
	}

	public string[] species =
	[
		"Feral", "Human",
	];

	public async Task<IEnumerable<string>> SpeciesSearch(string value)
	{
		// In real life use an asynchronous function for fetching data from an api.
		await Task.Delay(5);

		// if text is null or empty, show complete list
		if (string.IsNullOrEmpty(value)) return species;
		return species.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
	}
}
using Blazored.LocalStorage;
using MudBlazor;
using TomsApp.Models;

namespace TomsApp.Services;
public class CharacterService
{
	private readonly ILocalStorageService _localStorageService;
	private readonly ISnackbar _snackbar;

    public CharacterService(ILocalStorageService localStorageService, ISnackbar snackbar)
    {
        _localStorageService = localStorageService;
		_snackbar = snackbar;
    }

    private const string CHARACTERS = "Characters";

	public event Action? Updated;

	public List<Models.Character> Characters { get; set; } = new List<Models.Character>();

    public Models.Character Current { get; set; } = new Models.Character();

	public async Task ReadData()
	{
		try
		{
			Characters = await _localStorageService.GetItemAsync<List<Models.Character>>(CHARACTERS) ?? [];
			Updated?.Invoke();
		}
		catch (Exception)
		{
			await _localStorageService.RemoveItemsAsync(new string[] { CHARACTERS });
			Current = new();
		}
	}


	public async Task Save()
	{
		var characterPosition = Characters.FindIndex(c => c.Id == Current.Id);
		if (characterPosition == -1)
		{
			Characters.Add(Current);
			await UpdateStorage();
			return;
		}
		Characters[characterPosition] = Current;
		await UpdateStorage();
	}

	private async Task UpdateStorage()
	{
		await _localStorageService.SetItemAsync(CHARACTERS, Characters);
		_snackbar.Add("Saved Successfully", Severity.Success);
		Updated?.Invoke();
	}
}

using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Xml.Linq;
using TomsApp.Models;
using TomsApp.Services;

namespace TomsApp.Components.Pages;
public partial class CharacterList
{
	[Inject]
	private CharacterService _characterService { get; set; } = default!;

	[Inject]
	private IDialogService _dialogService { get; set; } = default!;

	List<Character> _characters = new List<Character>();



	protected override void OnInitialized()
	{
		_characterService.Updated += async () => await InvokeAsync(StateHasChanged);
	}
	private async Task ClearCharacterSelected(Character character)
	{
		var options = new DialogOptions { CloseOnEscapeKey = true };
		var result = await (await _dialogService.ShowAsync<WarningDialog>("Warning, " + character.Name + " will be deleted.", options)).Result;
		if (!result.Canceled && (bool)(result.Data ?? false))
		{
			_characters.Remove(character); // need to save too but the function is in the home component
			//await Save();
		}
	}

	private async Task ClearCharacter() // This probably isn't needed
	{
		_characterService.Current = new Character();
		//await Save();
	}
}

using Microsoft.AspNetCore.Components;
using MudBlazor;
using TomsApp.Models;
using TomsApp.Services;

namespace TomsApp.Components.Pages;
public partial class Home
{

	[Inject]
	private ISnackbar _snackbar { get; set; } = default!;

	[Inject]
	private CharacterService _characterService { get; set; } = default!;

	private string? _newWeapon;
	private string? _newOther;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender) return;
		await _characterService.ReadData();
		Character character;
		if (_characterService.CharacterList.ActiveCharacterId == Guid.Empty)
		{
			character = _characterService.CharacterList.Characters.LastOrDefault() ?? new();
			await _characterService.UpdateId(character);
		} else
		{
			character = _characterService.CharacterList.Characters.Find(c => c.Id == _characterService.CharacterList.ActiveCharacterId);
		}
		_characterService.Current = character ?? new();
		_characterService.Updated += async () => await InvokeAsync(StateHasChanged);
		StateHasChanged();
	}

	public async Task Save()
	{
		await _characterService.Save();
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
}

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


	[Inject]
	private IJSRuntime _js { get; set; } = default!;

	[Inject]
	private ILocalStorageService _localStorageService { get; set; } = default!;

	[Inject]
	private ISnackbar _snackbar { get; set; } = default!;

	[Inject]
	private CharacterService _characterService { get; set; } = default!;

	//private Character _character = new();
	private string? _newWeapon;
	private string? _newOther;



	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
			return;
		await _characterService.ReadData();
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

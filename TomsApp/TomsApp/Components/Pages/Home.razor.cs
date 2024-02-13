using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using TomsApp.Models;

namespace TomsApp.Components.Pages;
public partial class Home
{
    private const string CHARACTER = "Character";

    [Inject]
    private ILocalStorageService _localStorageService { get; set; } = default!;

    private Character _character = new();

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
    }
}

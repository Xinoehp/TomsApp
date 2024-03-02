using Microsoft.AspNetCore.Components;
using TomsApp.Services;

namespace TomsApp.Components.Layout;
public partial class NavigationMenu
{
	private bool _isDarkMode = true;
	private bool _open = false;

	[Inject]
	private CharacterService _characterService { get; set; } = default!;

	protected override void OnInitialized()
	{
		_characterService.Updated += async () => await InvokeAsync(StateHasChanged);
	}

	private void Toggle()
	{
		_open = !_open;
	}
}

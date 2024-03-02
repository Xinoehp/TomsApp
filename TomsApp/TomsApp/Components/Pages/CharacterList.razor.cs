using Microsoft.AspNetCore.Components;
using TomsApp.Services;

namespace TomsApp.Components.Pages;
public partial class CharacterList
{
	[Inject]
	private CharacterService _characterService { get; set; } = default!;

	protected override void OnInitialized()
	{
		_characterService.Updated += async () => await InvokeAsync(StateHasChanged);
	}
}

using MudBlazor;

namespace TomsApp.Components.Layout;
public partial class MainLayout
{

	private bool open = false;

	void toggle()
	{
		open = !open;
	}

	private async Task ClearCharacterSelected()
	{
		// run home.ClearCharacterSelected()?
	}

}

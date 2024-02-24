namespace TomsApp.Components.Layout;
public partial class MainLayout
{
	private bool _isDarkMode = true;
	private bool _open = false;

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

}

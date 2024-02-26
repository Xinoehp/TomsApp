namespace TomsApp.Services;
public class CharacterService
{
	public event Action? Updated;

    public Models.Character Current { get; set; } = new Models.Character();
}

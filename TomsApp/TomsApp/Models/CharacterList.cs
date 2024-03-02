namespace TomsApp.Models; 
public class CharacterList {
	public Guid ActiveCharacterId { get; set; } = Guid.Empty;
	public List<Character> Characters { get; set; } = [];
}

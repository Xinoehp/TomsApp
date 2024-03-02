namespace TomsApp.Models; 
public class Weapon {
    public string Name { get; set; } = String.Empty;
    public string Skill { get; set; } = String.Empty;
	public string Range { get; set; } = String.Empty;
	public string Damage { get; set; } = String.Empty;
	public int Clip { get; set; } = 0;
	public string Rules { get; set; } = String.Empty;
}

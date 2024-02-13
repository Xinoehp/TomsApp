namespace TomsApp.Models; 
public class Character {
    public string Name { get; set; } = string.Empty;

    public string Role { get; set; } = "Dick head";

    public Brawn Brawn { get; set; } = new();

    public Skill Hacking { get; set; } = new();
}

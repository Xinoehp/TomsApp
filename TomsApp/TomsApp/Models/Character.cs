namespace TomsApp.Models; 
public class Character {
    public string Name { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;

    public Brawn Brawn { get; set; } = new();

    public Brains Brains { get; set; } = new();

    public Perception Perception { get; set; } = new();

    public Reflexes Reflexes { get; set; } = new();

    public Allure Allure { get; set; } = new();

    public Guts Guts { get; set; } = new();

    public HitPoints Grit { get; set; } = new();
    public HitPoints Endurance { get; set; } = new();
    public HitPoints Pressure { get; set; } = new();
    public HitPoints Morale { get; set; } = new();
}

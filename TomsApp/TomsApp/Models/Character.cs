namespace TomsApp.Models; 
public class Character {

    // General
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;

    // Stats and Skills
    public Stat Brawn { get; set; } = new();
    public Stat Brains { get; set; } = new();
    public Stat Perception { get; set; } = new();
    public Stat Reflexes { get; set; } = new();
    public Stat Allure { get; set; } = new();
    public Stat Guts { get; set; } = new();
    public HitPoints Grit { get; set; } = new();

    // Hit Points
    public HitPoints Endurance { get; set; } = new();
    public HitPoints Pressure { get; set; } = new();
    public HitPoints Morale { get; set; } = new();

    // Armour and Weapons


    // Items and Other
}

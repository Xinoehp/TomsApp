namespace TomsApp.Models; 
public class Character {

    // General
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;

    // Stats and Skills
    public Stat Brawn { get; set; } = new Stat { Name = "Brawn" };
    public Stat Brains { get; set; } = new Stat { Name = "Brains" };
    public Stat Perception { get; set; } = new Stat { Name = "Perception" };
    public Stat Reflexes { get; set; } = new Stat { Name = "Reflexes" };
    public Stat Allure { get; set; } = new Stat { Name = "Allure" };
    public Stat Guts { get; set; } = new Stat { Name = "Guts" };

    // Hit Points
    public HitPoints Grit { get; set; } = new HitPoints { Name = "Grit" };
    public HitPoints Endurance { get; set; } = new HitPoints { Name = "Endurance" };
    public HitPoints Pressure { get; set; } = new HitPoints { Name = "Pressure" };
    public HitPoints Morale { get; set; } = new HitPoints { Name = "Morale" };

    // Other stats
    public int Disconnect { get; set; }
    public int Exp { get; set; }


    // Armour and Weapons
    public Armour Armour { get; set; } = new();
	public List<Weapon> Weapons { get; set; } = new List<Weapon>();

	// Items and Other
	public List<string> Others { get; set; } = new List<string>();
}

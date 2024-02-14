namespace TomsApp.Models; 
public class Perception {
    public int Stat { get; set; }

    public List<Skill> Skills { get; set; } = new List<Skill>();
}

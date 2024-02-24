namespace TomsApp.Models; 
public class Skill {
    public string Name { get; set; } = String.Empty;
    public bool Advanced { get; set; }
    public bool Specialised { get; set; } = false;
    public string RelatedStat { get; set; } = String.Empty;
    //public Stat RelatedStat { get; set; } = new Stat();
}

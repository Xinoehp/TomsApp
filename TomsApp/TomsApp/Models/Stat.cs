namespace TomsApp.Models;
public class Stat
{
	public string Name { get; set; } = string.Empty;
	public int Value { get; set; }
	public List<Skill> Skills { get; set; } = [];
}

using Microsoft.AspNetCore.Components;
using MudBlazor;
using TomsApp.Models;

namespace TomsApp.Components.Pages;
public partial class StatsAndSkills
{
	[Inject]
	private ISnackbar _snackbar { get; set; } = default!;

	[Parameter]
	[EditorRequired]
	public Stat Stat { get; set; } = new Stat();

	Skill _selectedSkill = new();

	public List<Skill> StatSkills =
	[
		// Brawn Skills
		new Skill { Name = "Climbing", RelatedStat = "Brawn" },
		new Skill { Name = "Heavy Weapons", RelatedStat = "Brawn" },
		new Skill { Name = "Lifting", RelatedStat = "Brawn" },
		new Skill { Name = "Swimming (A)", RelatedStat = "Brawn", Advanced = true },
		new Skill { Name = "Throwing", RelatedStat = "Brawn" },
		new Skill { Name = "Unarmed Combat", RelatedStat = "Brawn" },
		new Skill { Name = "Pilot Ghost-TAC (A)", RelatedStat = "Brawn", Advanced = true },

		// Reflexes Skills
		new Skill { Name = "Athletics", RelatedStat = "Reflexes" },
		new Skill { Name = "Firearms", RelatedStat = "Reflexes" },
		new Skill { Name = "Melee Weapons", RelatedStat = "Reflexes" },
		new Skill { Name = "Pilot Drone (A)", RelatedStat = "Reflexes", Advanced = true },
		new Skill { Name = "Ride Motorcycle (A)", RelatedStat = "Reflexes", Advanced = true },
		new Skill { Name = "Stealth", RelatedStat = "Reflexes" },
		new Skill { Name = "Sleight of Hand", RelatedStat = "Reflexes" },

		// Guts Skills
		new Skill { Name = "Gambling", RelatedStat = "Guts" },
		new Skill { Name = "Interrogate", RelatedStat = "Guts" },
		new Skill { Name = "Intimidate", RelatedStat = "Guts" },
		new Skill { Name = "Streetwise", RelatedStat = "Guts" },
		new Skill { Name = "Strategy (A)", RelatedStat = "Guts", Advanced = true },
		new Skill { Name = "Tracking (A)", RelatedStat = "Guts", Advanced = true },
		new Skill { Name = "Disguise", RelatedStat = "Guts" },

		// Brains Skills
		new Skill { Name = "Hacking (A)", RelatedStat = "Brains", Advanced = true },
		new Skill { Name = "Electronics (A)", RelatedStat = "Brains", Advanced = true },
		new Skill { Name = "Mechanical (A)", RelatedStat = "Brains", Advanced = true },
		new Skill { Name = "Medical (A)", RelatedStat = "Brains", Advanced = true },
		new Skill { Name = "Programming (A)", RelatedStat = "Brains", Advanced = true },
		new Skill { Name = "Weapons Tech (A)", RelatedStat = "Brains", Advanced = true },
		new Skill { Name = "Occult (A)", RelatedStat = "Brains", Advanced = true },


		// Allure Skills
		new Skill { Name = "Animal Handling (A)", RelatedStat = "Allure", Advanced = true },
		new Skill { Name = "Barter", RelatedStat = "Allure" },
		new Skill { Name = "Deceive", RelatedStat = "Allure" },
		new Skill { Name = "Leadership", RelatedStat = "Allure" },
		new Skill { Name = "Persuasion", RelatedStat = "Allure" },
		new Skill { Name = "Seduction", RelatedStat = "Allure" },
		new Skill { Name = "Perform (A)", RelatedStat = "Allure", Advanced = true },

		// Perception Skills
		new Skill { Name = "Awareness", RelatedStat = "Perception" },
		new Skill { Name = "Drive (A)", RelatedStat = "Perception", Advanced = true },
		new Skill { Name = "Lock Pick (A)", RelatedStat = "Perception", Advanced = true },
		new Skill { Name = "Meld (A)", RelatedStat = "Perception", Advanced = true },
		new Skill { Name = "Pilot Aircraft (A)", RelatedStat = "Perception", Advanced = true },
		new Skill { Name = "Surveillance", RelatedStat = "Perception" },
		new Skill { Name = "Lip Read (A)", RelatedStat = "Perception", Advanced = true },
	];

}
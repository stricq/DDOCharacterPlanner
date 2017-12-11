using System;
using System.Linq;

using STR.Common.Contracts;


namespace DdoCharacterPlanner.Domain.Enumerations {

  public class Ability : Enumeration {

    #region Constructor

    private Ability(int Value, string DisplayName) : base(Value, DisplayName) { }

    #endregion Constructor

    #region Properties

    public static readonly Ability Strength     = new Ability(0, "Strength");
    public static readonly Ability Dexterity    = new Ability(1, "Dexterity");
    public static readonly Ability Constitution = new Ability(2, "Constitution");
    public static readonly Ability Intelligence = new Ability(3, "Intelligence");
    public static readonly Ability Wisdom       = new Ability(4, "Wisdom");
    public static readonly Ability Charisma     = new Ability(5, "Charisma");

    #endregion Properties

    #region Domain Methods

    public string ShortName => DisplayName.Substring(0, 3);

    public int Modifier(int points) => (int)Math.Round(((double)points - 10) / 2);

    public static Ability FromShortName(string Name) => GetAll<Ability>().Single(a => a.ShortName == Name);

    #endregion Domain Methods

  }

}

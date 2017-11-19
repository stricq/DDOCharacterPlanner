using DdoCharacterPlanner.Domain.Contracts;

using STR.Common.Contracts;


namespace DdoCharacterPlanner.Domain.Enumerations {

  public class ClassName : Enumeration, IPastLifeEnumeration {

    #region Constructor

    private ClassName(int Value, string DisplayName) : base(Value, DisplayName) { }

    #endregion Constructor

    #region IPastLifeEnumeration Implementation

    public PastLifeType Type { get; } = PastLifeType.Class;

    #endregion IPastLifeEnumeration Implementation

    #region Properties

    public static readonly ClassName Artificer   = new ClassName( 0, "Artificer");
    public static readonly ClassName Barbarian   = new ClassName( 1, "Barbarian");
    public static readonly ClassName Bard        = new ClassName( 2, "Bard");
    public static readonly ClassName Cleric      = new ClassName( 3, "Cleric");
    public static readonly ClassName Druid       = new ClassName( 4, "Druid");
    public static readonly ClassName FavoredSoul = new ClassName( 5, "Favored Soul");
    public static readonly ClassName Fighter     = new ClassName( 6, "Fighter");
    public static readonly ClassName Monk        = new ClassName( 7, "Monk");
    public static readonly ClassName Paladin     = new ClassName( 8, "Paladin");
    public static readonly ClassName Ranger      = new ClassName( 9, "Ranger");
    public static readonly ClassName Rogue       = new ClassName(10, "Rogue");
    public static readonly ClassName Sorcerer    = new ClassName(11, "Sorcerer");
    public static readonly ClassName Warlock     = new ClassName(12, "Warlock");
    public static readonly ClassName Wizard      = new ClassName(13, "Wizard");

    #endregion Properties

  }

}

using DdoCharacterPlanner.Domain.Contracts;

using STR.Common.Contracts;


namespace DdoCharacterPlanner.Domain.Enumerations {

  public class RaceName : Enumeration, IPastLifeEnumeration {

    #region Constructor

    private RaceName(int Value, string DisplayName) : base(Value, DisplayName) { }

    #endregion Constructor

    #region IPastLifeEnumeration Implementation

    public PastLifeType Type { get; } = PastLifeType.Race;

    #endregion IPastLifeEnumeration Implementation

    #region Properties

    public static readonly RaceName Aasimar    = new RaceName( 0, "Aasimar");
    public static readonly RaceName Dragonborn = new RaceName( 1, "Dragonborn");
    public static readonly RaceName Drow       = new RaceName( 2, "Drow");
    public static readonly RaceName Dwarf      = new RaceName( 3, "Dwarf");
    public static readonly RaceName Elf        = new RaceName( 4, "Elf");
    public static readonly RaceName Gnome      = new RaceName( 5, "Gnome");
    public static readonly RaceName Halfling   = new RaceName( 6, "Halfling");
    public static readonly RaceName HalfElf    = new RaceName( 7, "Half-Elf");
    public static readonly RaceName HalfOrc    = new RaceName( 8, "Half-Orc");
    public static readonly RaceName Human      = new RaceName( 9, "Human");
    public static readonly RaceName Warforged  = new RaceName(10, "Warforged");

    public static readonly RaceName Bladeforged        = new IconicRaceName(11, "Bladeforged");
    public static readonly RaceName MorningLord        = new IconicRaceName(12, "Morninglord");
    public static readonly RaceName PurpleDragonKnight = new IconicRaceName(13, "Purple Dragon Knight");
    public static readonly RaceName ShadarKai          = new IconicRaceName(14, "Shadar-Kai");
    public static readonly RaceName DeepGnome          = new IconicRaceName(15, "Deep Gnome");
    public static readonly RaceName AasimarScourge     = new IconicRaceName(16, "Scourge");

    #endregion Properties

    #region Domain Properties

    public virtual bool IsIconic { get; } = false;

    #endregion Domain Properties

    #region Private Classes

    private sealed class IconicRaceName : RaceName {

      public IconicRaceName(int Value, string DisplayName) : base(Value, DisplayName) { }

      public override bool IsIconic { get; } = true;

    }

    #endregion Private Classes

  }

}

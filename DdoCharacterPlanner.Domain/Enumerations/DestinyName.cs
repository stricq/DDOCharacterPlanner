using DdoCharacterPlanner.Domain.Contracts;

using STR.Common.Contracts;


namespace DdoCharacterPlanner.Domain.Enumerations {

  public class DestinyName : Enumeration, IPastLifeEnumeration {

    #region Constructor

    private DestinyName(int Value, string DisplayName) : base(Value, DisplayName) { }

    #endregion Constructor

    #region IPastLifeEnumeration Implementation

    public PastLifeType Type { get; } = PastLifeType.Destiny;

    #endregion IPastLifeEnumeration Implementation

    #region Properties

    public static readonly DestinyName DraconicIncarnation  = new  ArcaneDestinyName( 0, "Draconic Incarnation");
    public static readonly DestinyName Fatesinger           = new  ArcaneDestinyName( 1, "Fatesinger");
    public static readonly DestinyName Magister             = new  ArcaneDestinyName( 2, "Magister");
    public static readonly DestinyName DivineCrusader       = new  DivineDestinyName( 3, "Divine Crusader");
    public static readonly DestinyName ExaltedAngel         = new  DivineDestinyName( 4, "Exalted Angel");
    public static readonly DestinyName UnyieldingSentinel   = new  DivineDestinyName( 5, "Unyielding Sentinel");
    public static readonly DestinyName GrandmasterOfFlowers = new MartialDestinyName( 6, "Grandmaster of Flowers");
    public static readonly DestinyName LegendaryDreadnought = new MartialDestinyName( 7, "Legendary Dreadnought");
    public static readonly DestinyName Shadowdancer         = new MartialDestinyName( 8, "Shadowdancer");
    public static readonly DestinyName FuryOfTheWild        = new  PrimalDestinyName( 9, "Fury of the Wild");
    public static readonly DestinyName PrimalAvatar         = new  PrimalDestinyName(10, "Primal Avatar");
    public static readonly DestinyName ShiradiChampion      = new  PrimalDestinyName(11, "Shiradi Champion");

    #endregion Properties

    #region Domain Properties

    public virtual DestinySphere Sphere { get; } = null;

    #endregion Domain Properties

    #region Private Classes

    private sealed class ArcaneDestinyName : DestinyName {

      public ArcaneDestinyName(int Value, string DisplayName) : base(Value, DisplayName) { }

      public override DestinySphere Sphere { get; } = DestinySphere.Arcane;

    }

    private sealed class DivineDestinyName : DestinyName {

      public DivineDestinyName(int Value, string DisplayName) : base(Value, DisplayName) { }

      public override DestinySphere Sphere { get; } = DestinySphere.Divine;

    }

    private sealed class MartialDestinyName : DestinyName {

      public MartialDestinyName(int Value, string DisplayName) : base(Value, DisplayName) { }

      public override DestinySphere Sphere { get; } = DestinySphere.Martial;

    }

    private sealed class PrimalDestinyName : DestinyName {

      public PrimalDestinyName(int Value, string DisplayName) : base(Value, DisplayName) { }

      public override DestinySphere Sphere { get; } = DestinySphere.Primal;

    }

    #endregion Private Classes

  }

}

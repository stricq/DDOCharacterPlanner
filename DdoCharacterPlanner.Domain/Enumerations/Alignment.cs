using STR.Common.Contracts;


namespace DdoCharacterPlanner.Domain.Enumerations {

  public class Alignment : Enumeration {

    #region Constructor

    private Alignment(int Value, string DisplayValue) : base(Value, DisplayValue) { }

    #endregion Constructor

    #region Properties

    public static readonly Alignment LawfulGood     = new Alignment(1, "Lawful Good");
    public static readonly Alignment LawfulNeutral  = new Alignment(2, "Lawful Neutral");
    public static readonly Alignment NeutralGood    = new Alignment(3, "Neutral Good");
    public static readonly Alignment TrueNeutral    = new Alignment(4, "True Neutral");
    public static readonly Alignment ChaoticGood    = new Alignment(5, "Chaotic Good");
    public static readonly Alignment ChaoticNeutral = new Alignment(6, "Chaotic Neutral");

    #endregion Properties

  }

}

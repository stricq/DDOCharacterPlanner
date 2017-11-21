using STR.Common.Contracts;


namespace DdoCharacterPlanner.Domain.Enumerations {

  public class DestinySphere : Enumeration {

    #region Constructor

    private DestinySphere(int Value, string DisplayValue) : base(Value, DisplayValue) { }

    #endregion Constructor

    #region Properties

    public static readonly DestinySphere Martial = new DestinySphere(1, "Martial");
    public static readonly DestinySphere Divine  = new DestinySphere(2, "Divine");
    public static readonly DestinySphere Primal  = new DestinySphere(3, "Primal");
    public static readonly DestinySphere Arcane  = new DestinySphere(4, "Arcane");

    #endregion Properties

  }

}

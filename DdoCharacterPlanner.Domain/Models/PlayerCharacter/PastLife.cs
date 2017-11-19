using DdoCharacterPlanner.Domain.Contracts;


namespace DdoCharacterPlanner.Domain.Models.PlayerCharacter {

  public class PastLife {

    #region Properties

    public IPastLifeEnumeration Name { get; set; }

    public int Count { get; set; }

    #endregion Properties

    #region Domain Properties

    public static int MaxCount { get; } = 3;

    #endregion Domain Properties

  }

}

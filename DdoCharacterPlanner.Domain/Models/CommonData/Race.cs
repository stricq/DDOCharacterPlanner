using DdoCharacterPlanner.Domain.Enumerations;


namespace DdoCharacterPlanner.Domain.Models.CommonData {

  public class Race {

    #region Properties

    public RaceName Name { get; set; }

    public string Description { get; set; }

    public int[] BaseAbilities { get; set; }

    public int BaseAbilityPoints { get; set; }

    #endregion Properties

    #region Domain Properties

    public bool IsIconic => Name.IsIconic;

    #endregion Domain Properties

  }

}

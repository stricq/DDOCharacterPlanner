using DdoCharacterPlanner.Domain.Enumerations;


namespace DdoCharacterPlanner.Domain.Models.CommonData {

  public class FeatRequirement {

    #region Properties

    public RaceName RaceName { get; set; }

    public ClassName ClassName { get; set; }

    public string Acquire { get; set; }

    public int Level { get; set; }

    #endregion Properties

  }

}

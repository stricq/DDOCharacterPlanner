using DdoCharacterPlanner.Domain.Models.CommonData;


namespace DdoCharacterPlanner.Domain.Models.PlayerCharacter {

  public class Level {

    #region Properties

    public int CharacterLevel { get; set; }

    public int ClassLevel { get; set; }

    public Class Class { get; set; }

    #endregion Properties

  }

}

using System.Collections.Generic;

using DdoCharacterPlanner.Domain.Enumerations;


namespace DdoCharacterPlanner.Domain.Models.CommonData {

  public class Skill {

    #region Properties

    public string Name { get; set; }

    public string Description { get; set; }

    public Ability KeyAbility { get; set; }

    public List<ClassName> PrimaryClassNames { get; set; }

    public List<ClassName> CrossClassNames { get; set; }

    public string Icon { get; set; }

    #endregion Properties

    #region Domain Properties

    public string IconFilename => $"{Name}.bmp";

    #endregion Domain Properties

  }

}

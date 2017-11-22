using System.Collections.Generic;

using DdoCharacterPlanner.Domain.Enumerations;


namespace DdoCharacterPlanner.Domain.Models.CommonData {

  public class Skill {

    #region Properties

    public string Name { get; set; }

    public string Description { get; set; }

    public Ability KeyAbility { get; set; }

    public List<ClassName> PrimaryClasses { get; set; }

    public List<ClassName> CrossClasses { get; set; }

    public string Icon { get; set; }

    #endregion Properties

  }

}

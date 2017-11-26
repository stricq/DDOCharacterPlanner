using System.Collections.Generic;


namespace DdoCharacterPlanner.Domain.Models.CommonData {

  public class Spell {

    #region Properties

    public string Name { get; set; }

    public string Description { get; set; }

    public List<SpellCasterLevel> CasterLevels { get; set; }

    public string School { get; set; }

    public List<string> Costs { get; set; }

    public List<string> Components { get; set; }

    public List<string> MetaMagics { get; set; }

    public string Range { get; set; }

    public List<string> Targets { get; set; }

    public string Duration { get; set; }

    public List<string> CoolDowns { get; set; }

    public string SavingThrow { get; set; }

    public string Resistance { get; set; }

    public string Icon { get; set; }

    public bool CanBeBought { get; set; }

    #endregion Properties

    #region Domain Properties

    public string IconFilename => $"{Icon}.bmp";

    #endregion Domain Properties

  }

}

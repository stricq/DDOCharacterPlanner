using System.Collections.Generic;

using DdoCharacterPlanner.Domain.Enumerations;
using DdoCharacterPlanner.Domain.Models.PlayerCharacter;


namespace DdoCharacterPlanner.Domain.Models.CommonData {

  public class Class {

    #region Constructor

    public Class() {
      BaseAttackBonus = new int[Character.MaxLevel];

      FortitudeSave = new int[Character.MaxLevel];
         ReflexSave = new int[Character.MaxLevel];
           WillSave = new int[Character.MaxLevel];

      SpellPoints = new int[Character.MaxLevel];
    }

    #endregion Constructor

    #region Properties

    public ClassName Name { get; set; }

    public string Description { get; set; }

    public int HitDie { get; set; }

    public int SkillPoints { get; set; }

    public int[] BaseAttackBonus { get; set; }

    public int[] FortitudeSave { get; set; }

    public int[] ReflexSave { get; set; }

    public int[] WillSave { get; set; }

    public int[] SpellPoints { get; set; }

    public List<Alignment> Alignments { get; set; }

    public DestinySphere StartingSphere { get; set; }

    #endregion Properties

    #region Domain Properties

    public string IconFilename => $"{Name}.bmp";

    #endregion Domain Properties

  }

}

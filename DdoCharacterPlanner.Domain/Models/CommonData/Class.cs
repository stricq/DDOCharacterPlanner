using System.Collections.Generic;

using DdoCharacterPlanner.Domain.Enumerations;
using DdoCharacterPlanner.Domain.Models.PlayerCharacter;


namespace DdoCharacterPlanner.Domain.Models.CommonData {

  public class Class {

    #region Constructor

    public Class() {
      BaseAttackBonuses = new int[Character.MaxLevel];

      FortitudeSaves = new int[Character.MaxLevel];
         ReflexSaves = new int[Character.MaxLevel];
           WillSaves = new int[Character.MaxLevel];

      SpellPoints = new int[Character.MaxLevel];
    }

    #endregion Constructor

    #region Properties

    public ClassName Name { get; set; }

    public string Description { get; set; }

    public int HitDie { get; set; }

    public int SkillPoints { get; set; }

    public int[] BaseAttackBonuses { get; set; }

    public int[] FortitudeSaves { get; set; }

    public int[] ReflexSaves { get; set; }

    public int[] WillSaves { get; set; }

    public int[] SpellPoints { get; set; }

    public List<Alignment> Alignments { get; set; }

    public DestinySphere StartingSphere { get; set; }

    #endregion Properties

    #region Domain Properties

    public string IconFilename => $"{Name}.bmp";

    #endregion Domain Properties

    #region Domain Methods

    public int GetBaseAttackBonusIncrease(int Level) => getPointIncrease(BaseAttackBonuses, Level);

    public int GetFortitudeSaveIncrease(int Level) => getPointIncrease(FortitudeSaves, Level);

    public int GetReflexSaveIncrease(int Level) => getPointIncrease(ReflexSaves, Level);

    public int GetWillSaveIncrease(int Level) => getPointIncrease(WillSaves, Level);

    public int GetSpellPointIncrease(int Level) => getPointIncrease(SpellPoints, Level);

    #endregion Domain Methods

    #region Private Methods

    private static int getPointIncrease(int[] points, int level) {
      level = level < Character.MinLevel ? Character.MinLevel
            : level > Character.MaxLevel ? Character.MaxLevel
            : level;

      level--;

      return level == 0 ? points[0] : points[level] - points[level - 1];
    }

    #endregion Private Methods

  }

}

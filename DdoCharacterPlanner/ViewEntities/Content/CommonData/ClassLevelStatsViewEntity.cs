using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewEntities.Content.CommonData {

  public class ClassLevelStatsViewEntity : ObservableObject {

    #region Private Fields

    private int level;

    private int baseAttackBonus;

    private int fortitudeSave;

    private int reflexSave;

    private int willSave;

    private int spellPoints;

    #endregion Private Fields

    #region Properties

    public int Level {
      get => level;
      set { SetField(ref level, value, () => Level); }
    }

    public int BaseAttackBonus {
      get => baseAttackBonus;
      set { SetField(ref baseAttackBonus, value, () => BaseAttackBonus); }
    }

    public int FortitudeSave {
      get => fortitudeSave;
      set { SetField(ref fortitudeSave, value, () => FortitudeSave); }
    }

    public int ReflexSave {
      get => reflexSave;
      set { SetField(ref reflexSave, value, () => ReflexSave); }
    }

    public int WillSave {
      get => willSave;
      set { SetField(ref willSave, value, () => WillSave); }
    }

    public int SpellPoints {
      get => spellPoints;
      set { SetField(ref spellPoints, value, () => SpellPoints); }
    }

    #endregion Properties

  }

}

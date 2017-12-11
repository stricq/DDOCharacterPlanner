using System;

using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewEntities.Content.CommonData {

  public class AbilityViewEntity : ObservableObject {

    #region Private Fields

    private string ability;

    private string shortName;

    private int points;

    private int modifier;

    #endregion Private Fields

    #region Properties

    public string Ability {
      get => ability;
      set { SetField(ref ability, value, () => Ability); }
    }

    public string ShortName {
      get => shortName;
      set { SetField(ref shortName, value, () => ShortName); }
    }

    public int Points {
      get => points;
      set { SetField(ref points, value, () => Points, () => PointsFormat); }
    }

    public string PointsFormat => $"{Points,3:##0}";

    public int Modifier {
      get => modifier;
      set { SetField(ref modifier, value, () => Modifier, () => ModifierFormat); }
    }

    public string ModifierFormat => $"{Modifier,3:+##0;-##0;0}";

    #endregion Properties

  }

}

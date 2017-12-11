using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using DdoCharacterPlanner.Domain.Enumerations;

using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewEntities.Content.CommonData {

  public class SkillViewEntity : ObservableObject {

    #region Private Fields

    private bool isChecked;

    private string name;

    private string keyAbility;

    private string description;

    private List<ClassName> primaryClassNames;

    private List<ClassName> crossClassNames;

    #endregion Private Fields

    #region Properties

    public bool IsChecked {
      get => isChecked;
      set { SetField(ref isChecked, value, () => IsChecked); }
    }

    public string Name {
      get => name;
      set { SetField(ref name, value, () => Name, () => IconFilename); }
    }

    public string KeyAbility {
      get => keyAbility;
      set { SetField(ref keyAbility, value, () => KeyAbility); }
    }

    public List<ClassName> PrimaryClassNames {
      get => primaryClassNames;
      set { SetField(ref primaryClassNames, value, () => PrimaryClassNames); }
    }

    public List<ClassName> CrossClassNames {
      get => crossClassNames;
      set { SetField(ref crossClassNames, value, () => CrossClassNames); }
    }

    public string Description {
      get => description;
      set { SetField(ref description, value, () => Description); }
    }

    public string PrimaryClassList => String.Join(", ", PrimaryClassNames.OrderBy(cn => cn.DisplayName));

    public string CrossClassList => String.Join(", ", CrossClassNames.OrderBy(cn => cn.DisplayName));

    public Uri IconFilename => new Uri(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"STR Programming Services\DDO Character Planner\Images\Skills", $"{Name}.bmp"));

    #endregion Properties

  }

}

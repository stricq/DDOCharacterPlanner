using System;
using System.Collections.ObjectModel;
using System.IO;

using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewEntities.Content.CommonData {

  public class RaceViewEntity : ObservableObject {

    #region Private Fields

    private bool isChecked;

    private string name;

    private string description;

    private ObservableCollection<AbilityViewEntity> baseAbilities;

    private int baseAbilityPoints;

    private bool isIconic;

    #endregion Private Fields

    #region Properties

    public bool IsChecked {
      get => isChecked;
      set { SetField(ref isChecked, value, () => IsChecked); }
    }

    public string Name {
      get => name;
      set { SetField(ref name, value, () => Name, () => MaleIconFilename, () => FemaleIconFilename); }
    }

    public string Description {
      get => description;
      set { SetField(ref description, value, () => Description); }
    }

    public ObservableCollection<AbilityViewEntity> BaseAbilities {
      get => baseAbilities;
      set { SetField(ref baseAbilities, value, () => BaseAbilities); }
    }

    public int BaseAbilityPoints {
      get => baseAbilityPoints;
      set { SetField(ref baseAbilityPoints, value, () => BaseAbilityPoints); }
    }

    public bool IsIconic {
      get => isIconic;
      set { SetField(ref isIconic, value, () => IsIconic, () => Iconic); }
    }

    public string Iconic => IsIconic ? "Iconic" : "Heroic";

    public Uri MaleIconFilename => new Uri(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"STR Programming Services\DDO Character Planner\Images\Races", $"{Name} Male.bmp"));

    public Uri FemaleIconFilename => new Uri(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"STR Programming Services\DDO Character Planner\Images\Races", $"{Name} Female.bmp"));

    #endregion Properties

  }

}

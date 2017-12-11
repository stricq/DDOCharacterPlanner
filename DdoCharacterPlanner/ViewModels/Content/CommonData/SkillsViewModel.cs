using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

using DdoCharacterPlanner.ViewEntities.Content.CommonData;

using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewModels.Content.CommonData {

  [Export]
  [ViewModel(nameof(SkillsViewModel))]
  public class SkillsViewModel : ObservableObject {

    #region Private Fields

    private ObservableCollection<SkillViewEntity> skills;

    private SkillViewEntity selected;

    #endregion Private Fields

    #region Properties

    public ObservableCollection<SkillViewEntity> Skills {
      get => skills;
      set { SetField(ref skills, value, () => Skills); }
    }

    public SkillViewEntity Selected {
      get => selected;
      set { SetField(ref selected, value, () => Selected); }
    }

    #endregion Properties

  }

}

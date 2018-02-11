using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

using DdoCharacterPlanner.ViewEntities.Content.CommonData;

using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewModels.Content.CommonData {

  [Export]
  [ViewModel(nameof(FeatsViewModel))]
  public sealed class FeatsViewModel : ObservableObject {

    #region Private Fields

    private ObservableCollection<FeatViewEntity> feats;

    private FeatViewEntity selected;

    #endregion Private Fields

    #region Properties

    public ObservableCollection<FeatViewEntity> Feats {
      get => feats;
      set { SetField(ref feats, value, () => Feats); }
    }

    public FeatViewEntity Selected {
      get => selected;
      set { SetField(ref selected, value, () => Selected); }
    }

    #endregion Properties

  }

}

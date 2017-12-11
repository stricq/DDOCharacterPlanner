using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

using DdoCharacterPlanner.ViewEntities.Content.CommonData;

using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewModels.Content.CommonData {

  [Export]
  [ViewModel(nameof(RacesViewModel))]
  public class RacesViewModel : ObservableObject {

    #region Private Fields

    private ObservableCollection<RaceViewEntity> races;

    private RaceViewEntity selected;

    #endregion Private Fields

    #region Properties

    public ObservableCollection<RaceViewEntity> Races {
      get => races;
      set { SetField(ref races, value, () => Races); }
    }

    public RaceViewEntity Selected {
      get => selected;
      set { SetField(ref selected, value, () => Selected); }
    }

    #endregion Properties

  }

}

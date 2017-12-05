using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;

using DdoCharacterPlanner.ViewEntities;

using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewModels {

  [Export]
  [ViewModel(nameof(PlannerViewModel))]
  public class PlannerViewModel : ObservableObject {

    #region Private Fields

    private RelayCommandAsync<RoutedEventArgs> loaded;

    private RelayCommand<CancelEventArgs> closing;

    private SettingsViewEntity settings;

    #endregion Private Fields

    #region Properties

    public RelayCommandAsync<RoutedEventArgs> Loaded {
      get => loaded;
      set { SetField(ref loaded, value, () => Loaded); }
    }

    public RelayCommand<CancelEventArgs> Closing {
      get => closing;
      set { SetField(ref closing, value, () => Closing); }
    }

    public SettingsViewEntity Settings {
      get => settings;
      set { SetField(ref settings, value, () => Settings); }
    }

    #endregion Properties

  }

}

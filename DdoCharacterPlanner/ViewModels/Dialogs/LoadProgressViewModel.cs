using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

using DdoCharacterPlanner.ViewEntities.Dialogs;

using STR.DialogView.Domain.Contracts;
using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewModels.Dialogs {

  [Export]
  [ViewModel(nameof(LoadProgressViewModel))]
  public class LoadProgressViewModel : ObservableObject, IDialogViewModel {

    #region Private Fields

    private ObservableCollection<LoadProgressViewEntity> loaders;

    #endregion Private Fields

    #region Properties

    public ObservableCollection<LoadProgressViewEntity> Loaders {
      get => loaders;
      set { SetField(ref loaders, value, () => Loaders); }
    }

    #endregion Properties

  }

}

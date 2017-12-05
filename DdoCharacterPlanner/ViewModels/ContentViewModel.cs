using System.ComponentModel.Composition;

using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewModels {

  [Export]
  [ViewModel(nameof(ContentViewModel))]
  public class ContentViewModel : ObservableObject {

    #region Private Fields

    private bool isPlannerVisible;

    private bool isCommonDataVisible;

    #endregion Private Fields

    #region Properties

    public bool IsPlannerVisible {
      get => isPlannerVisible;
      set { SetField(ref isPlannerVisible, value, () => IsPlannerVisible); }
    }

    public bool IsCommonDataVisible {
      get => isCommonDataVisible;
      set { SetField(ref isCommonDataVisible, value, () => IsCommonDataVisible); }
    }

    #endregion Properties

  }

}

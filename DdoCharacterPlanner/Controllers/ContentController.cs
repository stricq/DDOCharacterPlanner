using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

using DdoCharacterPlanner.ViewModels;

using STR.MvvmCommon.Contracts;


namespace DdoCharacterPlanner.Controllers {

  [Export(typeof(IController))]
  public class ContentController : IController {

    #region Private Fields

    private readonly ContentViewModel viewModel;

    private readonly MainMenuViewModel menuViewModel;

    #endregion Private Fields

    #region Constructor

    [ImportingConstructor]
    public ContentController(ContentViewModel ViewModel, MainMenuViewModel MenuViewModel) {
      viewModel = ViewModel;

      menuViewModel = MenuViewModel;
    }

    #endregion Constructor

    #region IController Implementation

    public async Task InitializeAsync() {
      menuViewModel.PropertyChanged += onMenuViewModelPropertyChanged;

      menuViewModel.IsCommonDataVisible = true;

      await Task.CompletedTask;
    }

    public int InitializePriority { get; } = 100;

    #endregion IController Implementation

    #region Private Methods

    private void onMenuViewModelPropertyChanged(object sender, PropertyChangedEventArgs args) {
      switch(args.PropertyName) {
          case "IsPlannerVisible": {
            menuViewModel.IsCommonDataVisible = !menuViewModel.IsPlannerVisible;

            viewModel.IsPlannerVisible = menuViewModel.IsPlannerVisible;

            break;
          }
          case "IsCommonDataVisible": {
            menuViewModel.IsPlannerVisible = !menuViewModel.IsCommonDataVisible;

            viewModel.IsCommonDataVisible = menuViewModel.IsCommonDataVisible;

            break;
          }
      }
    }

    #endregion Private Methods

  }

}

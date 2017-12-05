using System.ComponentModel.Composition;
using System.Threading.Tasks;

using DdoCharacterPlanner.ViewModels;
using DdoCharacterPlanner.ViewModels.Content;

using STR.MvvmCommon.Contracts;


namespace DdoCharacterPlanner.Controllers.Content {

  [Export(typeof(IController))]
  public class CharacterController : IController {

    #region Private Fields

    private readonly CharacterViewModel viewModel;

    private readonly MainMenuViewModel menuViewModel;

    #endregion Private Fields

    #region Constructor

    [ImportingConstructor]
    public CharacterController(CharacterViewModel ViewModel, MainMenuViewModel MenuViewModel) {
      viewModel = ViewModel;

      menuViewModel = MenuViewModel;
    }

    #endregion Constructor

    #region IController Implementation

    public async Task InitializeAsync() {
      await Task.CompletedTask;
    }

    public int InitializePriority { get; } = 500;

    #endregion IController Implementation

    #region Private Methods

    #endregion Private Methods

  }

}

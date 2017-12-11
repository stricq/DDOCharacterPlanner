using System.ComponentModel.Composition;
using System.Threading.Tasks;

using DdoCharacterPlanner.Domain.Contracts;
using DdoCharacterPlanner.Messages.Application;
using DdoCharacterPlanner.ViewModels.Content;

using STR.MvvmCommon.Contracts;


namespace DdoCharacterPlanner.Controllers.Content {

  [Export(typeof(IController))]
  public class CommonDataController : IController {

    #region Private Fields

    private readonly IMessenger messenger;

    private readonly ICommonData commonData;

    private readonly CommonDataViewModel viewModel;

    #endregion Private Fields

    #region Constructor

    [ImportingConstructor]
    public CommonDataController(CommonDataViewModel ViewModel, IMessenger Messenger, ICommonData CommonData) {
      viewModel = ViewModel;

      messenger = Messenger;

      commonData = CommonData;
    }

    #endregion Constructor

    #region IController Implementation

    public async Task InitializeAsync() {
      registerMessages();

      await Task.CompletedTask;
    }

    public int InitializePriority { get; } = 500;

    #endregion IController Implementation

    #region Messages

    private void registerMessages() {
      messenger.RegisterAsync<CommonDataLoadedMessage>(this, onCommonDataLoaded);
    }

    private async Task onCommonDataLoaded(CommonDataLoadedMessage message) {
      await Task.CompletedTask;
    }

    #endregion Messages

  }

}

﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

using DdoCharacterPlanner.Constants;
using DdoCharacterPlanner.Domain.Contracts;

using DdoCharacterPlanner.Messages.Application;
using DdoCharacterPlanner.ViewEntities.Dialogs;
using DdoCharacterPlanner.ViewModels;
using DdoCharacterPlanner.ViewModels.Dialogs;

using FontAwesome.WPF;

using STR.Common.Contracts;
using STR.Common.Extensions;
using STR.DialogView.Domain.Messages;

using STR.MvvmCommon;
using STR.MvvmCommon.Contracts;


namespace DdoCharacterPlanner.Controllers.Dialogs {

  [Export(typeof(IController))]
  public class LoadProgressController : IController {

    #region Private Fields

    private readonly IMessenger messenger;

    private readonly IAsyncService asyncService;

    private readonly ICommonDataStore commonDataStore;

    private readonly LoadProgressViewModel viewModel;

    private readonly MainMenuViewModel menuViewModel;

    #endregion Private Fields

    #region Constructor

    [ImportingConstructor]
    public LoadProgressController(LoadProgressViewModel ViewModel, MainMenuViewModel MenuViewModel, IMessenger Messenger, IAsyncService AsyncService, ICommonDataStore CommonDataStore) {
      viewModel = ViewModel;

      menuViewModel = MenuViewModel;

      messenger = Messenger;

      asyncService = AsyncService;

      commonDataStore = CommonDataStore;
    }

    #endregion Constructor

    #region IController Implementation

    public int InitializePriority { get; } = 50;

    public async Task InitializeAsync() {
      registerMessages();
      registerCommands();

      await Task.CompletedTask;
    }

    #endregion IController Implementation

    #region Messages

    private void registerMessages() {
      messenger.RegisterAsync<LoadProgressMessage>(this, onLoadProgressAsync);
    }

    private async Task onLoadProgressAsync(LoadProgressMessage message) {
      //
      // This method alwyas runs on a background thread.  Executed at program start only.
      //
      // Due to requirements of the Bitmap class (used by FontAwesome) the entities must be created on the
      // UI thread.
      //
      await asyncService.RunUiContext(() => viewModel.Loaders = buildEntityList());

      messenger.SendUi(new OpenDialogMessage { Name = DialogNames.LoadProgressDialog });

      await commonDataStore.LoadDataFilesAsync(onProgressCallbackAsync, false);
    }

    #endregion Messages

    #region Commands

    private void registerCommands() {
      menuViewModel.ReloadCommonData = new RelayCommandAsync(onReloadCommonDataExecute);
    }

    private async Task onReloadCommonDataExecute() {
      //
      // This method always runs on the UI thread.  Executed by user interaction.
      //
      messenger.Send(new CommonDataLoadingMessage());

      viewModel.Loaders = buildEntityList();

      messenger.Send(new OpenDialogMessage { Name = DialogNames.LoadProgressDialog });

      await commonDataStore.LoadDataFilesAsync(onProgressCallbackAsync, true);
    }

    #endregion Commands

    #region Private Methods

    private ObservableCollection<LoadProgressViewEntity> buildEntityList() {
      return new ObservableCollection<LoadProgressViewEntity>(commonDataStore.GetLoaderFiles().OrderBy(loader => loader).Select(loader => new LoadProgressViewEntity {
        StatusIcon  = FontAwesomeIcon.Spinner,
        StatusColor = new SolidColorBrush(Colors.BlueViolet),
        StatusSpin  = true,
        Loader      = loader
      }));
    }

    private async Task onProgressCallbackAsync(string message) {
      LoadProgressViewEntity entity = viewModel.Loaders.Single(loader => loader.Loader == message);

      await asyncService.RunUiContext(() => {
        entity.StatusIcon  = FontAwesomeIcon.Check;
        entity.StatusColor = new SolidColorBrush(Colors.LawnGreen);
        entity.StatusSpin  = false;
      });

      if (viewModel.Loaders.Any(loader => loader.StatusSpin)) return;

      await Task.Delay(TimeSpan.FromSeconds(1));

      messenger.SendUi(new CloseDialogMessage());

      messenger.SendAsync(new CommonDataLoadedMessage()).FireAndForget();
    }

    #endregion Private Methods

  }

}

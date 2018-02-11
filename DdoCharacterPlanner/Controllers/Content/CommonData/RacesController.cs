using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using DdoCharacterPlanner.Domain.Contracts;
using DdoCharacterPlanner.Domain.Enumerations;

using DdoCharacterPlanner.Messages.Application;
using DdoCharacterPlanner.ViewEntities.Content.CommonData;
using DdoCharacterPlanner.ViewModels.Content.CommonData;

using STR.Common.Contracts;
using STR.Common.Extensions;

using STR.MvvmCommon.Contracts;


namespace DdoCharacterPlanner.Controllers.Content.CommonData {

  [Export(typeof(IController))]
  public class RacesController : IController {

    #region Private Fields

    private readonly IMessenger messenger;
    private readonly IMapper    mapper;

    private readonly ICommonData commonData;

    private readonly RacesViewModel viewModel;

    #endregion Private Fields

    #region Constructor

    [ImportingConstructor]
    public RacesController(RacesViewModel ViewModel, IMessenger Messenger, IMapper Mapper, ICommonData CommonData) {
      viewModel = ViewModel;

      messenger = Messenger;
      mapper    = Mapper;

      commonData = CommonData;
    }

    #endregion Constructor

    #region IController Implementation

    public int InitializePriority { get; } = 100;

    public async Task InitializeAsync() {
      registerMessages();

      buildEntities(() => Enumeration.GetAll<RaceName>().Select(rn => new RaceViewEntity { Name = rn.DisplayName }).OrderBy(r => r.Name));

      await Task.CompletedTask;
    }

    #endregion IController Implementation

    #region Messages

    private void registerMessages() {
      messenger.Register<CommonDataLoadingMessage>(this, onCommonDataLoading);

      messenger.RegisterAsync<CommonDataLoadedMessage>(this, onCommonDataLoadedAsync);
    }

    private void onCommonDataLoading(CommonDataLoadingMessage message) {
      buildEntities(() => Enumeration.GetAll<RaceName>().Select(rn => new RaceViewEntity { Name = rn.DisplayName }).OrderBy(r => r.Name));
    }

    private async Task onCommonDataLoadedAsync(CommonDataLoadedMessage message) {
      buildEntities(() => mapper.Map<List<RaceViewEntity>>(commonData.Races).OrderBy(r => r.Name));

      await Task.CompletedTask;
    }

    #endregion Messages

    #region Private Methods

    private void onRaceViewEntityPropertyChanged(object sender, PropertyChangedEventArgs args) {
      if (!(sender is RaceViewEntity entity)) return;

      switch(args.PropertyName) {
        case "IsChecked": {
          if (entity.IsChecked) {
            viewModel.Selected = entity;

            viewModel.Races.Where(r => r != entity).ForEach(r => r.IsChecked = false);
          }

          break;
        }
      }
    }

    private void buildEntities(Func<IEnumerable<RaceViewEntity>> builder) {
      viewModel.Races?.ForEach(r => r.PropertyChanged -= onRaceViewEntityPropertyChanged);

      viewModel.Races = new ObservableCollection<RaceViewEntity>(builder());

      viewModel.Races.ForEach(r => {
        r.PropertyChanged += onRaceViewEntityPropertyChanged;

        r.IsChecked = r.Name == RaceName.Human.DisplayName;
      });
    }

    #endregion Private Methods

  }

}

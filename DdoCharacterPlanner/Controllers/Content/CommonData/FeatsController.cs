using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using DdoCharacterPlanner.Domain.Contracts;
using DdoCharacterPlanner.Domain.Models.CommonData;

using DdoCharacterPlanner.Messages.Application;
using DdoCharacterPlanner.ViewEntities.Content.CommonData;
using DdoCharacterPlanner.ViewModels.Content.CommonData;

using STR.Common.Extensions;

using STR.MvvmCommon.Contracts;


namespace DdoCharacterPlanner.Controllers.Content.CommonData {

  [Export(typeof(IController))]
  public sealed class FeatsController : IController {

    #region Private Fields

    private bool isSelf;

    private FeatViewEntity lastSelected;

    private readonly IMessenger messenger;
    private readonly IMapper    mapper;

    private readonly ICommonData commonData;

    private readonly FeatsViewModel viewModel;

    #endregion Private Fields

    #region Constructor

    [ImportingConstructor]
    public FeatsController(FeatsViewModel ViewModel, IMessenger Messenger, IMapper Mapper, ICommonData CommonData) {
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

      buildEntities(() => new List<FeatViewEntity>());

      await Task.CompletedTask;
    }

    #endregion IController Implementation

    #region Messages

    private void registerMessages() {
      messenger.Register<CommonDataLoadingMessage>(this, onCommonDataLoading);

      messenger.RegisterAsync<CommonDataLoadedMessage>(this, onCommonDataLoadedAsync);
    }

    private void onCommonDataLoading(CommonDataLoadingMessage message) {
      buildEntities(() => new List<FeatViewEntity>());
    }

    private async Task onCommonDataLoadedAsync(CommonDataLoadedMessage message) {
      buildEntities(() => buildFeatsTree(commonData.Feats.Where(f => f.IsParentHeader || String.IsNullOrEmpty(f.ParentName)).ToList()));

      await Task.CompletedTask;
    }

    #endregion Messages

    #region Private Methods

    private void onFeatViewEntityPropertyChanged(object sender, PropertyChangedEventArgs args) {
      if (isSelf) return;

      if (!(sender is FeatViewEntity entity)) return;

      switch(args.PropertyName) {
        case "IsSelected": {
          if (entity.IsSelected) {
            if (entity.IsParentHeader) {
              isSelf = true;

              entity.IsSelected = false;

              lastSelected.IsSelected = true;

              isSelf = false;

              entity = lastSelected;
            }

            viewModel.Selected = entity;

            viewModel.Feats.Traverse(fe => true).Where(c => c != entity).ForEach(c => c.IsSelected = false);
          }
          else lastSelected = entity;

          break;
        }
      }
    }

    private List<FeatViewEntity> buildFeatsTree(List<Feat> parents) {
      if (!parents.Any()) return new List<FeatViewEntity>();

      List<FeatViewEntity> entities = mapper.Map<List<FeatViewEntity>>(parents);

      entities.ForEach(fe => fe.Children = new ObservableCollection<FeatViewEntity>(buildFeatsTree(commonData.Feats.Where(f => f.ParentName == fe.Name).ToList())));

      return entities.OrderBy(fe => fe.Name).ToList();
    }

    private void buildEntities(Func<IEnumerable<FeatViewEntity>> builder) {
      viewModel.Feats?.ForEach(r => r.PropertyChanged -= onFeatViewEntityPropertyChanged);

      viewModel.Feats = new ObservableCollection<FeatViewEntity>(builder());

      viewModel.Feats.Traverse().ForEach(r => r.PropertyChanged += onFeatViewEntityPropertyChanged);

      FeatViewEntity entity = viewModel.Feats.FirstOrDefault(fe => !fe.IsParentHeader);

      if (entity != null) entity.IsSelected = true;
    }

    #endregion Private Methods

  }

}

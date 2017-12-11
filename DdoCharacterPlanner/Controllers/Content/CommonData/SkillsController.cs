using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using DdoCharacterPlanner.Domain.Contracts;

using DdoCharacterPlanner.Messages.Application;
using DdoCharacterPlanner.ViewEntities.Content.CommonData;
using DdoCharacterPlanner.ViewModels.Content.CommonData;

using STR.Common.Extensions;

using STR.MvvmCommon.Contracts;


namespace DdoCharacterPlanner.Controllers.Content.CommonData {

  [Export(typeof(IController))]
  public class SkillsController : IController {

    #region Private Fields

    private readonly IMessenger messenger;
    private readonly IMapper    mapper;

    private readonly ICommonData commonData;

    private readonly SkillsViewModel viewModel;

    #endregion Private Fields

    #region Constructor

    [ImportingConstructor]
    public SkillsController(SkillsViewModel ViewModel, IMessenger Messenger, IMapper Mapper, ICommonData CommonData) {
      viewModel = ViewModel;

      messenger = Messenger;
      mapper    = Mapper;

      commonData = CommonData;
    }

    #endregion Constructor

    #region IController Implementation

    public async Task InitializeAsync() {
      registerMessages();

      await Task.CompletedTask;
    }

    public int InitializePriority { get; } = 100;

    #endregion IController Implementation

    #region Messages

    private void registerMessages() {
      messenger.RegisterAsync<CommonDataLoadedMessage>(this, onCommonDataLoadedAsync);
    }

    private async Task onCommonDataLoadedAsync(CommonDataLoadedMessage message) {
      viewModel.Skills?.ForEach(c => c.PropertyChanged -= onSkillViewEntityPropertyChanged);

      viewModel.Skills = new ObservableCollection<SkillViewEntity>(mapper.Map<List<SkillViewEntity>>(commonData.Skills).OrderBy(c => c.Name));

      viewModel.Skills.ForEach(c => c.PropertyChanged += onSkillViewEntityPropertyChanged);

      viewModel.Skills[0].IsChecked = true;

      await Task.CompletedTask;
    }

    private void onSkillViewEntityPropertyChanged(object sender, PropertyChangedEventArgs args) {
      if (!(sender is SkillViewEntity entity)) return;

      switch(args.PropertyName) {
        case "IsChecked": {
          if (entity.IsChecked) {
            viewModel.Selected = entity;

            viewModel.Skills.Where(c => c != entity).ForEach(c => c.IsChecked = false);
          }

          break;
        }
      }
    }

    #endregion Messages

  }

}

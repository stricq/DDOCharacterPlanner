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
  public class ClassesController : IController {

    #region Private Fields

    private readonly IMessenger messenger;
    private readonly IMapper    mapper;

    private readonly ICommonData commonData;

    private readonly ClassesViewModel viewModel;

    #endregion Private Fields

    #region Constructor

    [ImportingConstructor]
    public ClassesController(ClassesViewModel ViewModel, IMessenger Messenger, IMapper Mapper, ICommonData CommonData) {
      viewModel = ViewModel;

      messenger = Messenger;
      mapper    = Mapper;

      commonData = CommonData;
    }

    #endregion Constructor

    #region IController Implementation

    public async Task InitializeAsync() {
      registerMessages();

      viewModel.Classes = new ObservableCollection<ClassViewEntity>(Enumeration.GetAll<ClassName>().Select(cn => new ClassViewEntity { Name = cn.DisplayName, IsChecked = cn == ClassName.Fighter }).OrderBy(c => c.Name));

      await Task.CompletedTask;
    }

    public int InitializePriority { get; } = 100;

    #endregion IController Implementation

    #region Messages

    private void registerMessages() {
      messenger.RegisterAsync<CommonDataLoadedMessage>(this, onCommonDataLoadedAsync);
    }

    private async Task onCommonDataLoadedAsync(CommonDataLoadedMessage message) {
      viewModel.Classes?.ForEach(c => c.PropertyChanged -= onRaceViewEntityPropertyChanged);

      viewModel.Classes = new ObservableCollection<ClassViewEntity>(mapper.Map<List<ClassViewEntity>>(commonData.Classes).OrderBy(c => c.Name));

      viewModel.Classes.ForEach(c => {
        c.PropertyChanged += onRaceViewEntityPropertyChanged;

        c.IsChecked = c.Name == ClassName.Fighter.DisplayName;
      });

      await Task.CompletedTask;
    }

    private void onRaceViewEntityPropertyChanged(object sender, PropertyChangedEventArgs args) {
      if (!(sender is ClassViewEntity entity)) return;

      switch(args.PropertyName) {
        case "IsChecked": {
          if (entity.IsChecked) {
            viewModel.Selected = entity;

            viewModel.Classes.Where(c => c != entity).ForEach(c => c.IsChecked = false);
          }

          break;
        }
      }
    }

    #endregion Messages

  }

}

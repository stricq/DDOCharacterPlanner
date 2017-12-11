using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

using DdoCharacterPlanner.ViewEntities.Content.CommonData;

using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewModels.Content.CommonData {

  [Export]
  [ViewModel(nameof(ClassesViewModel))]
  public class ClassesViewModel : ObservableObject {

    #region Private Fields

    private ObservableCollection<ClassViewEntity> classes ;

    private ClassViewEntity selected;

    #endregion Private Fields

    #region Properties

    public ObservableCollection<ClassViewEntity> Classes {
      get => classes;
      set { SetField(ref classes, value, () => Classes); }
    }

    public ClassViewEntity Selected {
      get => selected;
      set { SetField(ref selected, value, () => Selected); }
    }

    #endregion Properties

  }

}

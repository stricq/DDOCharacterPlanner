using System.Windows.Media;

using FontAwesome.WPF;

using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewEntities.Dialogs {

  public class LoadProgressViewEntity : ObservableObject {

    #region Private Fields

    private bool statusSpin;

    private string loader;

    private FontAwesomeIcon statusIcon;

    private SolidColorBrush statusColor;

    #endregion Private Fields

    #region Properties

    public bool StatusSpin {
      get => statusSpin;
      set { SetField(ref statusSpin, value, () => StatusSpin); }
    }

    public string Loader {
      get => loader;
      set { SetField(ref loader, value, () => Loader); }
    }

    public FontAwesomeIcon StatusIcon {
      get => statusIcon;
      set { SetField(ref statusIcon, value, () => StatusIcon); }
    }

    public SolidColorBrush StatusColor {
      get => statusColor;
      set { SetField(ref statusColor, value, () => StatusColor); }
    }

    #endregion Properties

  }

}

using System.ComponentModel.Composition;
using System.Windows.Controls;

using DdoCharacterPlanner.Constants;

using STR.DialogView.Domain.Contracts;

using STR.MvvmCommon;


namespace DdoCharacterPlanner.Views.Dialogs {

  [Export(typeof(IDialogViewLocator))]
  [ViewTag(Name=DialogNames.LoadProgressDialog)]
  public partial class LoadProgressView : UserControl, IDialogViewLocator {

    public LoadProgressView() {
      InitializeComponent();
    }

  }

}

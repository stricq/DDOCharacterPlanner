﻿using System.ComponentModel.Composition;

using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewModels {

  [Export]
  [ViewModel("MainMenuViewModel")]
  public class MainMenuViewModel : ObservableObject {

    #region Private Fields

    private RelayCommand clear;

    private RelayCommandAsync load;
    private RelayCommandAsync save;
    private RelayCommandAsync saveAs;

    private RelayCommand forumExport;

    private RelayCommand print;

    private RelayCommand exit;

    private RelayCommand itemBuilder;
    private RelayCommand equipmentBuilder;

    private RelayCommand showError;

    private RelayCommand about;

    #endregion Private Fields

    #region Properties

    public RelayCommand Clear {
      get => clear;
      set { SetField(ref clear, value, () => Clear); }
    }

    public RelayCommandAsync Load {
      get => load;
      set { SetField(ref load, value, () => Load); }
    }

    public RelayCommandAsync Save {
      get => save;
      set { SetField(ref save, value, () => Save); }
    }

    public RelayCommandAsync SaveAs {
      get => saveAs;
      set { SetField(ref saveAs, value, () => SaveAs); }
    }

    public RelayCommand ForumExport {
      get => forumExport;
      set { SetField(ref forumExport, value, () => ForumExport); }
    }

    public RelayCommand Print {
      get => print;
      set { SetField(ref print, value, () => Print); }
    }

    public RelayCommand Exit {
      get => exit;
      set { SetField(ref exit, value, () => Exit); }
    }

    public RelayCommand ItemBuilder {
      get => itemBuilder;
      set { SetField(ref itemBuilder, value, () => ItemBuilder); }
    }

    public RelayCommand EquipmentBuilder {
      get => equipmentBuilder;
      set { SetField(ref equipmentBuilder, value, () => EquipmentBuilder); }
    }

    public RelayCommand ShowError {
      get => showError;
      set { SetField(ref showError, value, () => ShowError); }
    }

    public RelayCommand About {
      get => about;
      set { SetField(ref about, value, () => About); }
    }

    #endregion Properties

  }

}

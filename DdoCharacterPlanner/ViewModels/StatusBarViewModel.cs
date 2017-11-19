using System.ComponentModel.Composition;

using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewModels
{

  [Export]
  [ViewModel("StatusBarViewModel")]
  public class StatusBarViewModel : ObservableObject {

    #region Private Fields

    private double memory;

    private string message;

    private string characterLevel;
    private string characterName;
    private string characterAlignment;
    private string characterGender;
    private string characterRace;

    private string characterClass1;
    private string characterClass2;
    private string characterClass3;

    private string characterEpic;

    #endregion Private Fields

    #region Properties

    public double Memory {
      get => memory;
      set { SetField(ref memory, value, () => Memory); }
    }

    public string Message {
      get => message;
      set { SetField(ref message, value, () => Message); }
    }

    public string CharacterLevel {
      get => characterLevel;
      set { SetField(ref characterLevel, value, () => CharacterLevel); }
    }

    public string CharacterName {
      get => characterName;
      set { SetField(ref characterName, value, () => CharacterName); }
    }

    public string CharacterAlignment {
      get => characterAlignment;
      set { SetField(ref characterAlignment, value, () => CharacterAlignment); }
    }

    public string CharacterGender {
      get => characterGender;
      set { SetField(ref characterGender, value, () => CharacterGender); }
    }

    public string CharacterRace {
      get => characterRace;
      set { SetField(ref characterRace, value, () => CharacterRace); }
    }

    public string CharacterClass1 {
      get => characterClass1;
      set {
        SetField(ref characterClass1, value, () => CharacterClass1);
      }
    }

    public string CharacterClass2 {
      get => characterClass2;
      set {
        SetField(ref characterClass2, value, () => CharacterClass2);
      }
    }

    public string CharacterClass3 {
      get => characterClass3;
      set {
        SetField(ref characterClass3, value, () => CharacterClass3);
      }
    }

    public string CharacterEpic {
      get => characterEpic;
      set {
        SetField(ref characterEpic, value, () => CharacterEpic);
      }
    }

    #endregion Properties

  }

}

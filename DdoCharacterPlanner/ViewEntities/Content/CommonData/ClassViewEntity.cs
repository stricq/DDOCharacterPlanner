using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

using DdoCharacterPlanner.Domain.Enumerations;

using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewEntities.Content.CommonData {

  public class ClassViewEntity : ObservableObject {

    #region Private Fields

    private bool isChecked;

    private string name;

    private string description;

    private string startingSphere;

    private int hitDie;

    private int skillPoints;

    private List<Alignment> alignments;

    private ObservableCollection<ClassLevelStatsViewEntity> levelStats;

    #endregion Private Fields

    #region Properties

    public bool IsChecked {
      get => isChecked;
      set { SetField(ref isChecked, value, () => IsChecked); }
    }

    public string Name {
      get => name;
      set { SetField(ref name, value, () => Name, () => IconFilename); }
    }

    public string Description {
      get => description;
      set { SetField(ref description, value, () => Description); }
    }

    public string StartingSphere {
      get => startingSphere;
      set { SetField(ref startingSphere, value, () => StartingSphere); }
    }

    public int HitDie {
      get => hitDie;
      set { SetField(ref hitDie, value, () => HitDie); }
    }

    public int SkillPoints {
      get => skillPoints;
      set { SetField(ref skillPoints, value, () => SkillPoints); }
    }

    public List<Alignment> Alignments {
      get => alignments;
      set { SetField(ref alignments, value, () => Alignments); }
    }

    public ObservableCollection<ClassLevelStatsViewEntity> LevelStats {
      get => levelStats;
      set { SetField(ref levelStats, value, () => LevelStats); }
    }

    public string AlignmentList => String.Join(", ", Alignments);

    public Uri IconFilename => new Uri(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"STR Programming Services\DDO Character Planner\Images\Classes", $"{Name}.bmp"));

    #endregion Properties

  }

}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

using STR.Common.Contracts;

using STR.MvvmCommon;


namespace DdoCharacterPlanner.ViewEntities.Content.CommonData {

  public class FeatViewEntity : ObservableObject, ITraversable<FeatViewEntity> {

    #region Private Fields

    private bool isSelected;

    private bool isParentHeader;

    private string name;

    private string parentName;

    private string description;

    private string icon;

    private List<string> tags;

    private List<string> locks;

    private ObservableCollection<FeatViewEntity> children;

    #endregion Private Fields

    #region Properties

    public bool IsSelected {
      get => isSelected;
      set { SetField(ref isSelected, value, () => IsSelected); }
    }

    public bool IsParentHeader {
      get => isParentHeader;
      set { SetField(ref isParentHeader, value, () => IsParentHeader); }
    }

    public string Name {
      get => name;
      set { SetField(ref name, value, () => Name); }
    }

    public string ParentName {
      get => parentName;
      set { SetField(ref parentName, value, () => ParentName); }
    }

    public string Description {
      get => description;
      set { SetField(ref description, value, () => Description); }
    }

    public string Icon {
      get => icon;
      set { SetField(ref icon, value, () => Icon); }
    }

    public List<string> Tags {
      get => tags;
      set { SetField(ref tags, value, () => Tags); }
    }

    public List<string> Locks {
      get => locks;
      set { SetField(ref locks, value, () => Locks); }
    }

    public ObservableCollection<FeatViewEntity> Children {
      get => children;
      set { SetField(ref children, value, () => Children); }
    }

    public string TagsList => String.Join(", ", Tags);

    public string LocksList => String.Join(", ", Locks);

    public Uri IconFilename => new Uri(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"STR Programming Services\DDO Character Planner\Images\Feats", $"{Icon}.bmp"));

    #endregion Properties

  }

}

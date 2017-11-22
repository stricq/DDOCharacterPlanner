﻿using System.Collections.Generic;
using System.Linq;


namespace DdoCharacterPlanner.Domain.Models.CommonData {

  public class Feat {

    #region Properties

    public string Name { get; set; }

    public string ParentName { get; set; }

    public string Description { get; set; }

    public string Icon { get; set; }

    public List<string> Locks { get; set; }

    public List<FeatNeed> NeedsAll { get; set; }

    public List<FeatNeed> NeedsOne { get; set; }

    public List<string> Tags { get; set; }

    public List<FeatRequirement> Requirements { get; set; }

    #endregion Properties

    #region Domain Properties

    public bool IsParentHeader { get; set; }

    #endregion Domain Properties

    #region Overrides

    public override int GetHashCode() {
      unchecked {
        int hashCode = Name != null ? Name.GetHashCode() : 0;

        hashCode = (hashCode * 397) ^ (ParentName   != null ? ParentName.GetHashCode()  : 0);
        hashCode = (hashCode * 397) ^ (Description  != null ? Description.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (Icon         != null ? Icon.GetHashCode()        : 0);
        hashCode = (hashCode * 397) ^ (Locks        != null ? getHashCode(Locks)        : 0);
        hashCode = (hashCode * 397) ^ (NeedsAll     != null ? getHashCode(NeedsAll)     : 0);
        hashCode = (hashCode * 397) ^ (NeedsOne     != null ? getHashCode(NeedsOne)     : 0);
        hashCode = (hashCode * 397) ^ (Tags         != null ? getHashCode(Tags)         : 0);
        hashCode = (hashCode * 397) ^ (Requirements != null ? getHashCode(Requirements) : 0);

        hashCode = (hashCode * 397) ^ IsParentHeader.GetHashCode();

        return hashCode;
      }
    }

    private static int getHashCode<T>(List<T> list) {
      if (!list.Any()) return 0;

      unchecked {
        return list.Aggregate(39, (current, item) => (current * 397) ^ item.GetHashCode());
      }
    }

    #endregion Overrides

  }

}

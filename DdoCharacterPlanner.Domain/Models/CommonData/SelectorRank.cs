using System.Collections.Generic;


namespace DdoCharacterPlanner.Domain.Models.CommonData {

  public class SelectorRank {

    #region Constructor

    public SelectorRank() {
      Modifies = new List<Modify>();
    }

    #endregion Constructor

    #region Properties

    public string Description { get; set; }

    public List<Need> NeedsAll { get; set; }

    public List<Need> NeedsOne { get; set; }

    public List<Need> Locks { get; set; }

    public List<Modify> Modifies { get; set; }

    #endregion Properties

  }

}

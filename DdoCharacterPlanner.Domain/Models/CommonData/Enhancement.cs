using System.Collections.Generic;


namespace DdoCharacterPlanner.Domain.Models.CommonData {

  public class Enhancement {

    #region Constructor

    public Enhancement() {
      SelectorRanks = new List<SelectorRank>();
    }

    #endregion Constructor

    #region Properties

    public string Name { get; set; }

    public string Description { get; set; }

    public string Icon { get; set; }

    public bool IsParentSelector { get; set; }

    public string ParentSelectorName { get; set; }

    public int TotalSelectorSlots { get; set; }

    public int SelectorSlot { get; set; }

    public List<SelectorRank> SelectorRanks { get; set; }

    public string TreeName { get; set; }

    public int TreeRow { get; set; }

    public int TreeColumn { get; set; }

    public bool IsPassive { get; set; }

    public int Cost { get; set; }

    #endregion Properties

    #region Domain Properties

    public string IconFilename => $"{Icon}.bmp";

    #endregion Domain Properties

  }

}

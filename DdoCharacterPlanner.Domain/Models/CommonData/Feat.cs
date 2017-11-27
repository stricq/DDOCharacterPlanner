using System.Collections.Generic;


namespace DdoCharacterPlanner.Domain.Models.CommonData {

  public class Feat {

    #region Properties

    public string Name { get; set; }

    public string ParentName { get; set; }

    public string Description { get; set; }

    public string Icon { get; set; }

    public List<string> Locks { get; set; }

    public List<Need> NeedsAll { get; set; }

    public List<Need> NeedsOne { get; set; }

    public List<string> Tags { get; set; }

    public List<FeatRequirement> Requirements { get; set; }

    #endregion Properties

    #region Domain Properties

    public bool IsParentHeader { get; set; }

    public string IconFilename => $"{Icon}.bmp";

    #endregion Domain Properties

  }

}

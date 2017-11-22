

namespace DdoCharacterPlanner.Domain.Models.CommonData {

  public class FeatNeed {

    #region Properties

    public string Type { get; set; }

    public string Name { get; set; }

    #endregion Properties

    #region Overrides

    public override int GetHashCode() {
      unchecked {
        return ((Type != null ? Type.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
      }
    }

    #endregion Overrides

  }

}

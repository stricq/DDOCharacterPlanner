using DdoCharacterPlanner.Domain.Enumerations;


namespace DdoCharacterPlanner.Domain.Models.CommonData {

  public class FeatRequirement {

    #region Properties

    public RaceName RaceName { get; set; }

    public ClassName ClassName { get; set; }

    public string Acquire { get; set; }

    public int Level { get; set; }

    #endregion Properties

    #region Overrides

    public override int GetHashCode() {
      unchecked {
        int hashCode = RaceName != null ? RaceName.GetHashCode() : 0;

        hashCode = (hashCode * 397) ^ (ClassName != null ? ClassName.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (Acquire   != null ? Acquire.GetHashCode()   : 0);
        hashCode = (hashCode * 397) ^ Level;

        return hashCode;
      }
    }

    #endregion Overrides

  }

}

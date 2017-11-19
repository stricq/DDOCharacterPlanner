using System.Collections.Generic;
using System.Linq;

using DdoCharacterPlanner.Domain.Enumerations;
using DdoCharacterPlanner.Domain.Models.PlayerCharacter;


namespace DdoCharacterPlanner.Domain.Extensions {

  public static class RaceExtensions {

    public static Race FindRace(this List<Race> Races, RaceName Name) {
      return Races.Single(race => race.Name == Name);
    }

  }

}

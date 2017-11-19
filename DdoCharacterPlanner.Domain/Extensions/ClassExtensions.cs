using System.Collections.Generic;
using System.Linq;

using DdoCharacterPlanner.Domain.Enumerations;
using DdoCharacterPlanner.Domain.Models.PlayerCharacter;


namespace DdoCharacterPlanner.Domain.Extensions {

  public static class ClassExtensions {

    public static Class FindClass(this List<Class> Classes, ClassName Name) {
      return Classes.Single(c => c.Name == Name);
    }

  }

}

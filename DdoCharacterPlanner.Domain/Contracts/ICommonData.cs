using System.Collections.Generic;

using DdoCharacterPlanner.Domain.Models.CommonData;


namespace DdoCharacterPlanner.Domain.Contracts {

  public interface ICommonData {

    List<Race> Races { get; }

    List<Class> Classes { get; }

    List<Skill> Skills { get; }

    List<Feat> Feats { get; }

    List<Spell> Spells { get; }

    List<Enhancement> Enhancements { get; }

    List<Destiny> Destinies { get; }

  }

}

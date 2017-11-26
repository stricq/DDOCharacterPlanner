using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DdoCharacterPlanner.Domain.Models.CommonData;


namespace DdoCharacterPlanner.Domain.Contracts {

  public interface ICommonDataStore {

    Task<bool> AreDataFilesPresent();

    Task LoadDataFilesAsync(Action<string> ProgressHandler);

    List<Race> Races { get; }

    List<Class> Classes { get; }

    List<Skill> Skills { get; }

    List<Feat> Feats { get; }

    List<Spell> Spells { get; }

  }

}

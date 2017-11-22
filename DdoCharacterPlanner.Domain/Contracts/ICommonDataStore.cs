using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DdoCharacterPlanner.Domain.Models.CommonData;


namespace DdoCharacterPlanner.Domain.Contracts {

  public interface ICommonDataStore {

    Task LoadDataFilesAsync(Action<string> ProgressHandler);

    List<Race> Races { get; }

    List<Class> Classes { get; }

    List<Feat> Feats { get; }

  }

}

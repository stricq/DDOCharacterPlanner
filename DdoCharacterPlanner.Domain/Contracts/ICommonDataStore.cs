using System;
using System.Threading.Tasks;


namespace DdoCharacterPlanner.Domain.Contracts {

  public interface ICommonDataStore {

    Task<bool> AreDataFilesPresent();

    Task LoadDataFilesAsync(Action<string> ProgressHandler);

  }

}

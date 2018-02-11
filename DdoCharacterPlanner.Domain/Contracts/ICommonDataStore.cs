using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DdoCharacterPlanner.Domain.Contracts {

  public interface ICommonDataStore {

    List<string> GetLoaderFiles();

    Task LoadDataFilesAsync(Func<string, Task> ProgressHandlerAsync, bool DownloadFilesFromWeb);

  }

}

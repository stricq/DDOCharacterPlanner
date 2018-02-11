using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DdoCharacterPlanner.Repository.Loaders {

  public interface IDataFileLoader {

    Type LoaderType { get; }

    string LoaderName { get; }

    Task<List<T>> LoadFromDataFileAsync<T>(string FilePath, string ImagePath, bool DownloadFilesFromWeb);

  }

}

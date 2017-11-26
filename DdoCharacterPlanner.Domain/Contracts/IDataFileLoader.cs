using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DdoCharacterPlanner.Domain.Contracts {

  public interface IDataFileLoader {

    Type LoaderType { get; }

    Task<List<T>> LoadFromDataFileAsync<T>(string FilePath, string ImagePath, IDataFileStore DataFileStore);

  }

}

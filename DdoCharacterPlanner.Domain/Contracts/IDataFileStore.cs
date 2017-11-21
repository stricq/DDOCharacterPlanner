using System;
using System.Collections.Generic;


namespace DdoCharacterPlanner.Domain.Contracts {

  public interface IDataFileStore {

    void StoreToDatabase<T>(Func<List<T>, List<T>> Updater);

  }

}

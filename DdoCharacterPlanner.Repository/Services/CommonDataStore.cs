using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using DdoCharacterPlanner.Domain.Contracts;
using DdoCharacterPlanner.Domain.Models.CommonData;


namespace DdoCharacterPlanner.Repository.Services {

  [Export(typeof(ICommonDataStore))]
  [Export(typeof(IDataFileStore))]
  public class CommonDataStore : ICommonDataStore, IDataFileStore {

    #region Private Fields

    private readonly List<IDataFileLoader> loaders;

    #endregion Private Fields

    #region Constructor

    [ImportingConstructor]
    public CommonDataStore([ImportMany] IEnumerable<IDataFileLoader> Loaders) {
      loaders = Loaders.ToList();
    }

    #endregion Constructor

    #region ICommonDataStore Implementation

    public async Task LoadDataFilesAsync(Action<string> ProgressHandler) {
      ProgressHandler?.Invoke("Loading Races...");

      Races = await loadDatafileAsync<Race>();

      ProgressHandler?.Invoke("Loading Classes...");

      Classes = await loadDatafileAsync<Class>();

      ProgressHandler?.Invoke("Loading Skills...");

      Skills = await loadDatafileAsync<Skill>();

      ProgressHandler?.Invoke("Loading Feats...");

      Feats = await loadDatafileAsync<Feat>();

      ProgressHandler?.Invoke("Loading Spells...");

//    Spells = await loadDatafileAsync<Spell>();

      ProgressHandler?.Invoke("Loading Enhancements...");

//    Enhancements = await loadDatafileAsync<Enhancement>();

      ProgressHandler?.Invoke("Loading Destinies...");

//    Destinies = await loadDatafileAsync<Destiny>();

      ProgressHandler?.Invoke("Ready");
    }

    public List<Race> Races { get; private set; }

    public List<Class> Classes { get; private set; }

    public List<Skill> Skills { get; private set; }

    public List<Feat> Feats { get; private set; }

    #endregion ICommonDataStore Implementation

    #region IDataFileStore Implementation

    public void StoreToDatabase<T>(Func<List<T>, List<T>> Updater) {
    }

    #endregion IDataFileStore Implementation

    #region Private Methods

    private async Task<List<T>> loadDatafileAsync<T>() {
      string dataFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"STR Programming Services\DDO Character Planner\Data Files");

      if (!await Task.Run(() => Directory.Exists(dataFilePath))) await Task.Run(() => Directory.CreateDirectory(dataFilePath));

      List<T> items = await loaders.Single(loader => loader.LoaderType == typeof(T)).LoadFromDataFileAsync<T>(dataFilePath, this);

      return items;
    }

    #endregion Private Methods

  }

}

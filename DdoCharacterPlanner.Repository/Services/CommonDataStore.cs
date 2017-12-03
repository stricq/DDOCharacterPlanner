using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using DdoCharacterPlanner.Domain.Contracts;
using DdoCharacterPlanner.Domain.Models.CommonData;

using DdoCharacterPlanner.Repository.Loaders;

using STR.Common.Extensions;


namespace DdoCharacterPlanner.Repository.Services {

  [Export(typeof(ICommonDataStore))]
  [Export(typeof(ICommonData))]
  public class CommonDataStore : ICommonDataStore, ICommonData {

    #region Private Fields

    private readonly string rootFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"STR Programming Services\DDO Character Planner");

    private readonly List<IDataFileLoader> loaders;

    #endregion Private Fields

    #region Constructor

    [ImportingConstructor]
    public CommonDataStore([ImportMany] IEnumerable<IDataFileLoader> Loaders) {
      loaders = Loaders.ToList();
    }

    #endregion Constructor

    #region ICommonDataStore Implementation

    public List<string> GetLoaderFiles() {
      return loaders.Select(loader => loader.LoaderName).ToList();
    }

    public async Task LoadDataFilesAsync(Action<string> ProgressHandler) {
      List<Task> tasks = new List<Task> {
        loadDataFileAsync<Race>(ProgressHandler) .ContinueWith(task => Races   = task.IsCompleted ? task.Result : new List<Race>() ),
        loadDataFileAsync<Class>(ProgressHandler).ContinueWith(task => Classes = task.IsCompleted ? task.Result : new List<Class>()),
        loadDataFileAsync<Skill>(ProgressHandler).ContinueWith(task => Skills  = task.IsCompleted ? task.Result : new List<Skill>()),
        loadDataFileAsync<Feat>(ProgressHandler) .ContinueWith(task => Feats   = task.IsCompleted ? task.Result : new List<Feat>() ),
        loadDataFileAsync<Spell>(ProgressHandler).ContinueWith(task => Spells  = task.IsCompleted ? task.Result : new List<Spell>()),

        loadDataFileAsync<Destiny>(ProgressHandler).ContinueWith(task => Destinies = task.IsCompleted ? task.Result : new List<Destiny>()),

        loadDataFileAsync<Enhancement>(ProgressHandler).ContinueWith(task => Enhancements = task.IsCompleted ? task.Result : new List<Enhancement>())
      };

      await tasks.ForEachAsync(task => task);
    }

    #endregion ICommonDataStore Implementation

    #region ICommonData Implementation

    public List<Race> Races { get; private set; }

    public List<Class> Classes { get; private set; }

    public List<Skill> Skills { get; private set; }

    public List<Feat> Feats { get; private set; }

    public List<Spell> Spells { get; private set; }

    public List<Enhancement> Enhancements { get; private set; }

    public List<Destiny> Destinies { get; private set; }

    #endregion ICommonData Implementation

    #region Private Methods

    private async Task<List<T>> loadDataFileAsync<T>(Action<string> progressHandler) {
      string  dataFilePath = Path.Combine(rootFilePath, "Data Files");
      string imageFilePath = Path.Combine(rootFilePath, "Images");

      IDataFileLoader loader = loaders.Single(l => l.LoaderType == typeof(T));

      List<T> items = await loader.LoadFromDataFileAsync<T>(dataFilePath, imageFilePath);

      progressHandler?.Invoke(loader.LoaderName);

      return items;
    }

    #endregion Private Methods

  }

}

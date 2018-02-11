using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using DdoCharacterPlanner.Domain.Contracts;
using DdoCharacterPlanner.Domain.Models.CommonData;

using DdoCharacterPlanner.Repository.Loaders;


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

    public async Task LoadDataFilesAsync(Func<string, Task> ProgressHandlerAsync, bool DownloadFilesFromWeb) {
      List<Task> tasks = new List<Task> {
        loadDataFileAsync<Race> (ProgressHandlerAsync, DownloadFilesFromWeb).ContinueWith(task => Races   = task.IsCompleted ? task.Result : new List<Race>() ),
        loadDataFileAsync<Class>(ProgressHandlerAsync, DownloadFilesFromWeb).ContinueWith(task => Classes = task.IsCompleted ? task.Result : new List<Class>()),
        loadDataFileAsync<Skill>(ProgressHandlerAsync, DownloadFilesFromWeb).ContinueWith(task => Skills  = task.IsCompleted ? task.Result : new List<Skill>()),
        loadDataFileAsync<Feat> (ProgressHandlerAsync, DownloadFilesFromWeb).ContinueWith(task => Feats   = task.IsCompleted ? task.Result : new List<Feat>() ),
        loadDataFileAsync<Spell>(ProgressHandlerAsync, DownloadFilesFromWeb).ContinueWith(task => Spells  = task.IsCompleted ? task.Result : new List<Spell>()),

        loadDataFileAsync<Destiny>    (ProgressHandlerAsync, DownloadFilesFromWeb).ContinueWith(task => Destinies    = task.IsCompleted ? task.Result : new List<Destiny>()),
        loadDataFileAsync<Enhancement>(ProgressHandlerAsync, DownloadFilesFromWeb).ContinueWith(task => Enhancements = task.IsCompleted ? task.Result : new List<Enhancement>())
      };

      await Task.WhenAll(tasks);
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

    private async Task<List<T>> loadDataFileAsync<T>(Func<string, Task> progressHandlerAsync, bool downloadFilesFromWeb) {
      string  dataFilePath = Path.Combine(rootFilePath, "Data Files");
      string imageFilePath = Path.Combine(rootFilePath, "Images");

      IDataFileLoader loader = loaders.Single(l => l.LoaderType == typeof(T));

      List<T> items = await loader.LoadFromDataFileAsync<T>(dataFilePath, imageFilePath, downloadFilesFromWeb);

      if (progressHandlerAsync != null) await progressHandlerAsync(loader.LoaderName);

      return items;
    }

    #endregion Private Methods

  }

}

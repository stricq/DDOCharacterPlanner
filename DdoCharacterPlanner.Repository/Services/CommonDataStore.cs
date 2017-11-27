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

    public async Task<bool> AreDataFilesPresent() {
      string  dataFilePath = Path.Combine(rootFilePath, "Data Files");
      string imageFilePath = Path.Combine(rootFilePath, "Images");

      return await Task.Run(() => Directory.Exists(dataFilePath))
          && await Task.Run(() => Directory.Exists(imageFilePath));
    }

    public async Task LoadDataFilesAsync(Action<string> ProgressHandler) {
      List<Task> tasks = new List<Task> {
        loadDataFileAsync<Race>() .ContinueWith(task => { Races   = task.IsCompleted ? task.Result : new List<Race>();  ProgressHandler?.Invoke("Races");   }),
        loadDataFileAsync<Class>().ContinueWith(task => { Classes = task.IsCompleted ? task.Result : new List<Class>(); ProgressHandler?.Invoke("Classes"); }),
        loadDataFileAsync<Skill>().ContinueWith(task => { Skills  = task.IsCompleted ? task.Result : new List<Skill>(); ProgressHandler?.Invoke("Skills");  }),
        loadDataFileAsync<Feat>() .ContinueWith(task => { Feats   = task.IsCompleted ? task.Result : new List<Feat>();  ProgressHandler?.Invoke("Feats");   }),
        loadDataFileAsync<Spell>().ContinueWith(task => { Spells  = task.IsCompleted ? task.Result : new List<Spell>(); ProgressHandler?.Invoke("Spells");  }),

        loadDataFileAsync<Enhancement>().ContinueWith(task => { Enhancements = task.IsCompleted ? task.Result : new List<Enhancement>(); ProgressHandler?.Invoke("Enhancements"); })
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

    #endregion ICommonData Implementation

    #region Private Methods

    private async Task<List<T>> loadDataFileAsync<T>() {
      string  dataFilePath = Path.Combine(rootFilePath, "Data Files");
      string imageFilePath = Path.Combine(rootFilePath, "Images");

      List<T> items = await loaders.Single(loader => loader.LoaderType == typeof(T)).LoadFromDataFileAsync<T>(dataFilePath, imageFilePath);

      return items;
    }

    #endregion Private Methods

  }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using DdoCharacterPlanner.Domain.Enumerations;
using DdoCharacterPlanner.Domain.Models.CommonData;

using STR.Common.Contracts;
using STR.Common.Extensions;


namespace DdoCharacterPlanner.Repository.Loaders {

  [Export(typeof(IDataFileLoader))]
  public class RaceFileLoader : DataFileLoaderBase, IDataFileLoader {

    #region Private Fields

    private const string Filename = "RaceFile.txt";

    private const string FileUrl = "https://raw.githubusercontent.com/stricq/DDOCharPlannerV4/master/DataFiles/RaceFile.txt";

    private const string ImageUrl = "https://raw.githubusercontent.com/stricq/DDOCharPlannerV4/master/Graphics/Races";

    #endregion Private Fields

    #region IDataFileLoader Implementation

    public Type LoaderType => typeof(Race);

    public string LoaderName => "Races";

    public async Task<List<T>> LoadFromDataFileAsync<T>(string FilePath, string ImagePath, bool DownloadFilesFromWeb) {
      HttpClient client = new HttpClient();

      string file = Path.Combine(FilePath, Filename);

      await VerifyAndDownloadAsync(client, file, FileUrl, DownloadFilesFromWeb);

      StreamReader stream = new StreamReader(file);

      List<Race> races = await ReadDataFileAsync<Race>(stream, (race, property, value) => {
        switch(property) {
          case "RACENAME": {
            race.Name = Enumeration.FromDisplayName<RaceName>(value.Replace("p-G", "p G"));

            race.BaseAbilityPoints = race.IsIconic ? 32 : 28;

            break;
          }
          case "DESCRIPTION": {
            race.Description = value;

            break;
          }
          case "BASESTATS": {
            race.BaseAbilities = ToIntArray(value);

            break;
          }
          case "ADVANCEMENT": {
            break;
          }
          default: {
            Console.WriteLine($"Encountered unexpected property '{property}' while reading {Filename}");

            break;
          }
        }
      });

      stream.Close();

      await races.ForEachAsync(race => {
        string   malePath = Path.Combine(ImagePath, "Races", race.MaleIconFilename);
        string femalePath = Path.Combine(ImagePath, "Races", race.FemaleIconFilename);

        string   maleUrl = $"{ImageUrl}/{race.Name.GetRemoteName("Male")}";
        string femaleUrl = $"{ImageUrl}/{race.Name.GetRemoteName("Female")}";
        //
        // ReSharper disable AccessToDisposedClosure - everything is awaited
        //
        List<Task> tasks = new List<Task> {
          VerifyAndDownloadAsync(client,   malePath,   maleUrl, DownloadFilesFromWeb),
          VerifyAndDownloadAsync(client, femalePath, femaleUrl, DownloadFilesFromWeb),
        };

        return Task.WhenAll(tasks);
      });

      client.Dispose();

      return races.Cast<T>().ToList();
    }

    #endregion IDataFileLoader Implementation

  }

}

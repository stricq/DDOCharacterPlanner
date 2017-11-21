using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using DdoCharacterPlanner.Domain.Contracts;
using DdoCharacterPlanner.Domain.Enumerations;
using DdoCharacterPlanner.Domain.Models.PlayerCharacter;

using STR.Common.Contracts;


namespace DdoCharacterPlanner.Repository.Loaders {

  [Export(typeof(IDataFileLoader))]
  public class RaceFileLoader : DataFileLoaderBase, IDataFileLoader {

    #region Private Fields

    private const string Filename = "RaceFile.txt";

    private const string FileUrl = "https://raw.githubusercontent.com/DDOCharPlanner/DDOCharPlannerV4/master/DataFiles/RaceFile.txt";

    #endregion Private Fields

    #region IDataFileLoader Implementation

    public Type LoaderType => typeof(Race);

    public async Task<List<T>> LoadFromDataFileAsync<T>(string FilePath, IDataFileStore DataFileStore) {
      string file = Path.Combine(FilePath, Filename);

      await VerifyAndDownloadAsync(file, FileUrl);

      StreamReader stream = new StreamReader(file);

      List<Race> races = await ReadDataFileAsync<Race>(stream, (race, property, value) => {
        switch(property) {
          case "RACENAME": {
            race.Name = Enumeration.FromDisplayName<RaceName>(value);

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
        }
      });

      races = races.Where(r => r.Name != null).ToList();

      DataFileStore.StoreToDatabase<Race>(dbRaces => {
        List<Race> newRaces = new List<Race>();

        foreach(Race race in races) {
          Race dbRace = dbRaces.SingleOrDefault(r => r.Name == race.Name);

          if (dbRace != null) {
            dbRace.Description       = race.Description;
            dbRace.BaseAbilityPoints = race.BaseAbilityPoints;
          }
          else newRaces.Add(race);
        }

        return newRaces;
      });

      return races.Cast<T>().ToList();
    }

    #endregion IDataFileLoader Implementation

  }

}

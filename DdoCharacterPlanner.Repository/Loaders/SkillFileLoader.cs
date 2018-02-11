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
  public class SkillFileLoader : DataFileLoaderBase, IDataFileLoader {

    #region Private Fields

    private const string Filename = "SkillFile.txt";

    private const string FileUrl = "https://raw.githubusercontent.com/stricq/DDOCharPlannerV4/master/DataFiles/SkillFile.txt";

    private const string ImageUrl = "https://raw.githubusercontent.com/stricq/DDOCharPlannerV4/master/Graphics/Skills";

    #endregion Private Fields

    #region IDataFileLoader Members

    public Type LoaderType => typeof(Skill);

    public string LoaderName => "Skills";

    public async Task<List<T>> LoadFromDataFileAsync<T>(string FilePath, string ImagePath, bool DownloadFilesFromWeb) {
      HttpClient client = new HttpClient();

      string file = Path.Combine(FilePath, Filename);

      await VerifyAndDownloadAsync(client, file, FileUrl, DownloadFilesFromWeb);

      StreamReader stream = new StreamReader(file);

      List<Skill> skills = await ReadDataFileAsync<Skill>(stream, (skill, property, value) => {
        switch(property) {
          case "SKILLNAME": {
            skill.Name = value;

            break;
          }
          case "DESCRIPTION": {
            skill.Description = value;

            break;
          }
          case "ICON": {
            skill.Icon = value;

            break;
          }
          case "KEYABILITY": {
            skill.KeyAbility = Ability.FromShortName(value);

            break;
          }
          case "PRIMARY": {
            skill.PrimaryClassNames = ToStringList(value).Select(Enumeration.FromDisplayName<ClassName>).ToList();

            break;
          }
          case "CROSS": {
            skill.CrossClassNames = ToStringList(value).Select(Enumeration.FromDisplayName<ClassName>).ToList();

            break;
          }
          default: {
            Console.WriteLine($"Encountered unexpected property '{property}' while reading {Filename}");

            break;
          }
        }
      });

      stream.Close();

      await skills.ForEachAsync(skill => {
        string path = Path.Combine(ImagePath, "Skills", skill.IconFilename);

        string url = $"{ImageUrl}/{skill.Icon}.bmp";
        //
        // ReSharper disable once AccessToDisposedClosure - everything is awaited
        //
        return VerifyAndDownloadAsync(client, path, url, DownloadFilesFromWeb);
      });

      client.Dispose();

      return skills.Cast<T>().ToList();
    }

    #endregion IDataFileLoader Members

  }

}

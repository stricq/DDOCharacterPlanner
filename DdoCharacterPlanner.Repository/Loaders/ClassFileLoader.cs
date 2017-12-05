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
  public class ClassFileLoader : DataFileLoaderBase, IDataFileLoader {

    #region Private Fields

    private const string Filename = "ClassFile.txt";

    private const string FileUrl = "https://raw.githubusercontent.com/stricq/DDOCharPlannerV4/master/DataFiles/ClassFile.txt";

    private const string ImageUrl = "https://raw.githubusercontent.com/stricq/DDOCharPlannerV4/master/Graphics/Classes";

    #endregion Private Fields

    #region IDataFileLoader Members

    public Type LoaderType => typeof(Class);

    public string LoaderName => "Classes";

    public async Task<List<T>> LoadFromDataFileAsync<T>(string FilePath, string ImagePath) {
      HttpClient client = new HttpClient();

      string file = Path.Combine(FilePath, Filename);

      await VerifyAndDownloadAsync(client, file, FileUrl);

      StreamReader stream = new StreamReader(file);

      List<Class> classes = await ReadDataFileAsync<Class>(stream, (@class, property, value) => {
        switch(property) {
          case "CLASSNAME": {
            @class.Name = Enumeration.FromDisplayName<ClassName>(value);

            break;
          }
          case "DESCRIPTION": {
            @class.Description = value;

            break;
          }
          case "HITDIE": {
            @class.HitDie = Int32.Parse(value);

            break;
          }
          case "SKILLPOINTS": {
            @class.SkillPoints = Int32.Parse(value);

            break;
          }
          case "BAB": {
            @class.BaseAttackBonus = ToIntArray(value);

            break;
          }
          case "FORTSAVE": {
            @class.FortitudeSave = ToIntArray(value);

            break;
          }
          case "REFSAVE": {
            @class.ReflexSave = ToIntArray(value);

            break;
          }
          case "WILLSAVE": {
            @class.WillSave = ToIntArray(value);

            break;
          }
          case "SPELLPOINTS": {
            @class.SpellPoints = ToIntArray(value);

            break;
          }
          case "ALIGNMENT": {
            @class.Alignments = ToStringList(value).Select(Enumeration.FromDisplayName<Alignment>).ToList();

            break;
          }
          case "STARTINGSPHERE": {
            @class.StartingSphere = Enumeration.FromDisplayName<DestinySphere>(value);

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

      await classes.ForEachAsync(@class => {
        string path = Path.Combine(ImagePath, "Classes", $"{@class.IconFilename}");

        string url = $"{ImageUrl}/{@class.IconFilename}";
        //
        // ReSharper disable once AccessToDisposedClosure - everything is awaited
        //
        return VerifyAndDownloadAsync(client, path, url);
      });

      client.Dispose();

      return classes.Cast<T>().ToList();
    }

    #endregion IDataFileLoader Members

  }

}

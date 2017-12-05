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
  public class FeatFileLoader : DataFileLoaderBase, IDataFileLoader {

    #region Private Fields

    private const string Filename = "FeatsFile.txt";

    private const string FileUrl = "https://raw.githubusercontent.com/stricq/DDOCharPlannerV4/master/DataFiles/FeatsFile.txt";

    private const string ImageUrl = "https://raw.githubusercontent.com/stricq/DDOCharPlannerV4/master/Graphics/Feats";

    #endregion Private Fields

    #region IDataFileLoader Members

    public Type LoaderType => typeof(Feat);

    public string LoaderName => "Feats";

    public async Task<List<T>> LoadFromDataFileAsync<T>(string FilePath, string ImagePath) {
      HttpClient client = new HttpClient();

      string file = Path.Combine(FilePath, Filename);

      await VerifyAndDownloadAsync(client, file, FileUrl);

      StreamReader stream = new StreamReader(file);

      List<Feat> feats = await ReadDataFileAsync<Feat>(stream, (feat, property, value) => {
        if (feat.Requirements == null) feat.Requirements = new List<FeatRequirement>();

        switch(property) {
          case "PARENTHEADING": {
            feat.ParentName = value;

            break;
          }
          case "FEATNAME": {
            feat.Name = value;

            break;
          }
          case "FEATDESCRIPTION": {
            feat.Description = value;

            break;
          }
          case "ICON": {
            feat.Icon = value;

            break;
          }
          case "LOCK": {
            feat.Locks = ToStringList(value);

            break;
          }
          case "NEEDSALL": {
            feat.NeedsAll = buildNeedsList(value);

            break;
          }
          case "NEEDSONE": {
            feat.NeedsOne = buildNeedsList(value);

            break;
          }
          case "FEATTAG": {
            feat.Tags = ToStringList(value);

            break;
          }
          case "RACELIST":
          case "CLASSLIST":
          case "ACQUIRE":
          case "LEVEL": {
            buildRequirementsList(property, value, feat);

            break;
          }
          case "VERIFIED": {
            break;
          }
          default: {
            Console.WriteLine($"Encountered unexpected property '{property}' while reading {Filename}");

            break;
          }
        }
      });

      stream.Close();

      await feats.Where(feat => !String.IsNullOrEmpty(feat.Icon)).GroupBy(feat => feat.Icon).ForEachAsync(grp => {
        string path = Path.Combine(ImagePath, "Feats", grp.First().IconFilename);

        string url = $"{ImageUrl}/{grp.First().IconFilename}";
        //
        // ReSharper disable once AccessToDisposedClosure - everything is awaited
        //
        return VerifyAndDownloadAsync(client, path, url).ContinueWith(task => {
          if (task.IsCompleted && !task.Result) grp.ForEach(feat => feat.Icon = "NoImage");
        });
      });

      feats.AddRange(feats.Where(f => !String.IsNullOrEmpty(f.ParentName))
                          .GroupBy(f => f.ParentName)
                          .Select(g => new Feat { Name = g.Key, IsParentHeader = true }));

      client.Dispose();

      return feats.Cast<T>().ToList();
    }

    #endregion

    #region Private Methods

    private static List<Need> buildNeedsList(string value) {
      List<string> needs = ToStringList(value);

      return needs.Select(need => {
        string[] parts = need.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

        return new Need {
          Type = parts[0],
          Name = String.Join(" ", parts.Skip(1))
        };
      }).ToList();
    }

    private static void buildRequirementsList(string property, string value, Feat feat) {
      if (String.IsNullOrEmpty(value)) return;

      List<string> parts = ToStringList(value);

      if (!feat.Requirements.Any()) for(int i = 0; i < parts.Count; ++i) feat.Requirements.Add(new FeatRequirement());

      feat.Requirements = feat.Requirements.Zip(parts, (a, b) => {
        switch(property) {
          case "RACELIST": {
            a.RaceName = Enumeration.FromDisplayName<RaceName>(b);

            break;
          }
          case "CLASSLIST": {
            a.ClassName = Enumeration.FromDisplayName<ClassName>(b);

            break;
          }
          case "ACQUIRE": {
            a.Acquire = b;

            break;
          }
          case "LEVEL": {
            a.Level = Int32.Parse(b);

            break;
          }
        }

        return a;
      }).ToList();
    }

    #endregion Private Methods

  }

}

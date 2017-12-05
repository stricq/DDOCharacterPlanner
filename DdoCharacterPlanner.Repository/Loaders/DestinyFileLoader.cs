

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using DdoCharacterPlanner.Domain.Models.CommonData;

using STR.Common.Extensions;


namespace DdoCharacterPlanner.Repository.Loaders {

  [Export(typeof(IDataFileLoader))]
  public class DestinyFileLoader : DataFileLoaderBase, IDataFileLoader {

    #region Private Fields

    private const string Filename = "DestinyFile.txt";

    private const string FileUrl = "https://raw.githubusercontent.com/stricq/DDOCharPlannerV4/master/DataFiles/DestinyFile.txt";

    private const string ImageUrl = "https://raw.githubusercontent.com/stricq/DDOCharPlannerV4/master/Graphics/Destinies";

    #endregion Private Fields

    #region IDataFileLoader Implementation

    public Type LoaderType => typeof(Destiny);

    public string LoaderName => "Destinies";

    public async Task<List<T>> LoadFromDataFileAsync<T>(string FilePath, string ImagePath) {
      HttpClient client = new HttpClient();

      bool isParentSelector = false;

      string file = Path.Combine(FilePath, Filename);

      await VerifyAndDownloadAsync(client, file, FileUrl);

      StreamReader stream = new StreamReader(file);

      List<Destiny> destinies = await ReadDataFileAsync<Destiny>(stream, (destiny, property, value) => {
        if (destiny == null) {
          if (property.Equals("MULTIPLE DESTINY SELECTOR", StringComparison.InvariantCultureIgnoreCase)) isParentSelector = true;

          if (property.Equals("ENDOFMULTI", StringComparison.InvariantCultureIgnoreCase)) isParentSelector = false;

          return;
        }

        switch(property) {
          case "MULTI": {
            destiny.ParentSelectorName = value;

            break;
          }
          case "MSLOT": {
            destiny.SelectorSlot = Int32.Parse(value);

            break;
          }
          case "NAME": {
            destiny.Name             = value;
            destiny.IsParentSelector = isParentSelector;

            break;
          }
          case "DESC": {
            destiny.Description = value;

            break;
          }
          case "TREE": {
            destiny.TreeName = value;

            break;
          }
          case "TIER": {
            destiny.TreeRow = Int32.Parse(value);

            break;
          }
          case "SLOT": {
            destiny.TreeColumn = Int32.Parse(value);

            break;
          }
          case "TYPE": {
            destiny.IsPassive = value.Equals("Passive", StringComparison.InvariantCultureIgnoreCase);

            break;
          }
          case "COST": {
            destiny.Cost = Int32.Parse(value);

            break;
          }
          case "RANKS": {
            destiny.TotalSelectorSlots = Int32.Parse(value);

            for(int i = 0; i < destiny.TotalSelectorSlots; ++i) destiny.SelectorRanks.Add(new SelectorRank());

            break;
          }
          case "DESC1": {
            destiny.SelectorRanks[0].Description = value;

            break;
          }
          case "REQ1": {
            destiny.SelectorRanks[0].NeedsAll = buildNeedsList(value);

            break;
          }
          case "REQONE1": {
            destiny.SelectorRanks[0].NeedsOne = buildNeedsList(value);

            break;
          }
          case "LOCK1": {
            destiny.SelectorRanks[0].Locks = buildNeedsList(value);

            break;
          }
          case "MOD1":
          case "MODTYPE1":
          case "MODNAME1":
          case "MODVALUE1": {
            buildModifyList(property.Replace("1", null), value, destiny.SelectorRanks[0]);

            break;
          }
          case "DESC2": {
            destiny.SelectorRanks[1].Description = value;

            break;
          }
          case "REQ2": {
            destiny.SelectorRanks[1].NeedsAll = buildNeedsList(value);

            break;
          }
          case "REQONE2": {
            destiny.SelectorRanks[1].NeedsOne = buildNeedsList(value);

            break;
          }
          case "LOCK2": {
            destiny.SelectorRanks[1].Locks = buildNeedsList(value);

            break;
          }
          case "MOD2":
          case "MODTYPE2":
          case "MODNAME2":
          case "MODVALUE2": {
            buildModifyList(property.Replace("2", null), value, destiny.SelectorRanks[1]);

            break;
          }
          case "DESC3": {
            destiny.SelectorRanks[2].Description = value;

            break;
          }
          case "REQ3": {
            destiny.SelectorRanks[2].NeedsAll = buildNeedsList(value);

            break;
          }
          case "REQONE3": {
            destiny.SelectorRanks[2].NeedsOne = buildNeedsList(value);

            break;
          }
          case "LOCK3": {
            destiny.SelectorRanks[2].Locks = buildNeedsList(value);

            break;
          }
          case "MOD3":
          case "MODTYPE3":
          case "MODNAME3":
          case "MODVALUE3": {
            buildModifyList(property.Replace("3", null), value, destiny.SelectorRanks[2]);

            break;
          }
          case "ICON": {
            destiny.Icon = value;

            break;
          }
          default: {
            Console.WriteLine($"Encountered unexpected property '{property}' while reading {Filename} on {destiny.Name}");

            break;
          }
        }
      }, true);

      stream.Close();

      await destinies.Where(enhancement => !String.IsNullOrEmpty(enhancement.Icon)).GroupBy(enhancement => enhancement.Icon).ForEachAsync(grp => {
        string path = Path.Combine(ImagePath, "Destinies", grp.First().IconFilename);

        string url = $"{ImageUrl}/{grp.First().IconFilename}";
        //
        // ReSharper disable once AccessToDisposedClosure - everything is awaited
        //
        return VerifyAndDownloadAsync(client, path, url).ContinueWith(task => {
          if (task.IsCompleted && !task.Result) grp.ForEach(feat => feat.Icon = "NoImage");
        });
      });

      client.Dispose();

      return destinies.Cast<T>().ToList();
    }

    #endregion IDataFileLoader Implementation

    #region Private Methods

    private static List<Need> buildNeedsList(string value) {
      List<string> needs = ToStringList(value);

      return needs.Select(need => {
        string[] parts = need.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

        string type;

        switch(parts[0]) {
          case "EH:": {
            type = "Enhancement";

            break;
          }
          case "EHM:": {
            type = "Enhancement Selector";

            break;
          }
          default: {
            type = parts[0].Replace(":", null);

            break;
          }
        }

        return new Need {
          Type = type,
          Name = String.Join(" ", parts.Skip(1))
        };
      }).ToList();
    }

    private static void buildModifyList(string property, string value, SelectorRank rank) {
      if (String.IsNullOrEmpty(value)) return;

      List<string> parts = ToStringList(value);

      if (!rank.Modifies.Any()) for(int i = 0; i < parts.Count; ++i) rank.Modifies.Add(new Modify());

      rank.Modifies = rank.Modifies.Zip(parts, (a, b) => {
        switch(property) {
          case "MOD": {
            a.Type = b;

            break;
          }
          case "MODTYPE": {
            a.BonusType = b;

            break;
          }
          case "MODNAME": {
            a.Name = b;

            break;
          }
          case "MODVALUE": {
            a.Value = Int32.Parse(b);

            break;
          }
        }

        return a;
      }).ToList();
    }

    #endregion Private Methods

  }

}

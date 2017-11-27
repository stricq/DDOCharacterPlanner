using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using DdoCharacterPlanner.Domain.Contracts;
using DdoCharacterPlanner.Domain.Models.CommonData;

using STR.Common.Extensions;


namespace DdoCharacterPlanner.Repository.Loaders {

  [Export(typeof(IDataFileLoader))]
  public class EnhancementFileLoader : DataFileLoaderBase, IDataFileLoader {

    #region Private Fields

    private const string Filename = "EnhancementFile.txt";

    private const string FileUrl = "https://raw.githubusercontent.com/DDOCharPlanner/DDOCharPlannerV4/master/DataFiles/EnhancementFile.txt";

    private const string ImageUrl = "https://raw.githubusercontent.com/DDOCharPlanner/DDOCharPlannerV4/master/Graphics/Enhancements";

    #endregion Private Fields

    #region IDataFileLoader Implementation

    public Type LoaderType => typeof(Enhancement);

    public async Task<List<T>> LoadFromDataFileAsync<T>(string FilePath, string ImagePath) {
      bool isParentSelector = false;

      string file = Path.Combine(FilePath, Filename);

      await VerifyAndDownloadAsync(file, FileUrl);

      StreamReader stream = new StreamReader(file);

      List<Enhancement> enhancements = await ReadDataFileAsync<Enhancement>(stream, (enhancement, property, value) => {
        if (enhancement == null) {
          if (property.Equals("MULTIPLE ENHANCEMENT SELECTORS", StringComparison.InvariantCultureIgnoreCase)) isParentSelector = true;

          if (property.Equals("ENDOFMULTI", StringComparison.InvariantCultureIgnoreCase)) isParentSelector = false;

          return;
        }

        switch(property) {
          case "MULTI": {
            enhancement.ParentSelectorName = value;

            break;
          }
          case "MSLOT": {
            enhancement.SelectorSlot = Int32.Parse(value);

            break;
          }
          case "NAME": {
            enhancement.Name             = value;
            enhancement.IsParentSelector = isParentSelector;

            break;
          }
          case "DESC": {
            enhancement.Description = value;

            break;
          }
          case "TREE": {
            enhancement.TreeName = value;

            break;
          }
          case "LEVEL": {
            enhancement.TreeRow = Int32.Parse(value);

            break;
          }
          case "SLOT": {
            enhancement.TreeColumn = Int32.Parse(value);

            break;
          }
          case "TYPE": {
            enhancement.IsPassive = value.Equals("Passive", StringComparison.InvariantCultureIgnoreCase);

            break;
          }
          case "COST": {
            enhancement.Cost = Int32.Parse(value);

            break;
          }
          case "RANKS": {
            enhancement.TotalSelectorSlots = Int32.Parse(value);

            for(int i = 0; i < enhancement.TotalSelectorSlots; ++i) enhancement.SelectorRanks.Add(new SelectorRank());

            break;
          }
          case "DESC1": {
            enhancement.SelectorRanks[0].Description = value;

            break;
          }
          case "REQ1": {
            enhancement.SelectorRanks[0].NeedsAll = buildNeedsList(value);

            break;
          }
          case "REQONE1": {
            enhancement.SelectorRanks[0].NeedsOne = buildNeedsList(value);

            break;
          }
          case "LOCK1": {
            enhancement.SelectorRanks[0].Locks = buildNeedsList(value);

            break;
          }
          case "MOD1":
          case "MODNAME1":
          case "MODVALUE1": {
            buildModifyList(property.Replace("1", null), value, enhancement.SelectorRanks[0]);

            break;
          }
          case "DESC2": {
            enhancement.SelectorRanks[1].Description = value;

            break;
          }
          case "REQ2": {
            enhancement.SelectorRanks[1].NeedsAll = buildNeedsList(value);

            break;
          }
          case "REQONE2": {
            enhancement.SelectorRanks[1].NeedsOne = buildNeedsList(value);

            break;
          }
          case "LOCK2": {
            enhancement.SelectorRanks[1].Locks = buildNeedsList(value);

            break;
          }
          case "MOD2":
          case "MODNAME2":
          case "MODVALUE2": {
            buildModifyList(property.Replace("2", null), value, enhancement.SelectorRanks[1]);

            break;
          }
          case "DESC3": {
            enhancement.SelectorRanks[2].Description = value;

            break;
          }
          case "REQ3": {
            enhancement.SelectorRanks[2].NeedsAll = buildNeedsList(value);

            break;
          }
          case "REQONE3": {
            enhancement.SelectorRanks[2].NeedsOne = buildNeedsList(value);

            break;
          }
          case "LOCK3": {
            enhancement.SelectorRanks[2].Locks = buildNeedsList(value);

            break;
          }
          case "MOD3":
          case "MODNAME3":
          case "MODVALUE3": {
            buildModifyList(property.Replace("3", null), value, enhancement.SelectorRanks[2]);

            break;
          }
          case "ICON": {
            enhancement.Icon = value;

            break;
          }
          default: {
            Console.WriteLine($"Encountered unexpected property '{property}' while reading {Filename} on {enhancement.Name}");

            break;
          }
        }
      }, true);

      await enhancements.Where(enhancement => !String.IsNullOrEmpty(enhancement.Icon)).GroupBy(enhancement => enhancement.Icon).ForEachAsync(grp => {
        string path = Path.Combine(ImagePath, "Enhancements", grp.First().IconFilename);

        string url = $"{ImageUrl}/{grp.First().IconFilename}";

        return VerifyAndDownloadAsync(path, url).ContinueWith(task => {
          if (task.IsCompleted && !task.Result) grp.ForEach(feat => feat.Icon = "NoImage");
        });
      });

      return enhancements.Cast<T>().ToList();
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

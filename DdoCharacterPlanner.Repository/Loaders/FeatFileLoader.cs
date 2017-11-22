using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using DdoCharacterPlanner.Domain.Contracts;
using DdoCharacterPlanner.Domain.Enumerations;
using DdoCharacterPlanner.Domain.Models.CommonData;

using STR.Common.Contracts;


namespace DdoCharacterPlanner.Repository.Loaders {

  [Export(typeof(IDataFileLoader))]
  public class FeatFileLoader : DataFileLoaderBase, IDataFileLoader {

    #region Private Fields

    private const string Filename = "FeatsFile.txt";

    private const string FileUrl = "https://raw.githubusercontent.com/DDOCharPlanner/DDOCharPlannerV4/master/DataFiles/FeatsFile.txt";

    #endregion Private Fields

    #region IDataFileLoader Members

    public Type LoaderType => typeof(Feat);

    public async Task<List<T>> LoadFromDataFileAsync<T>(string FilePath, IDataFileStore DataFileStore) {
      string file = Path.Combine(FilePath, Filename);

      await VerifyAndDownloadAsync(file, FileUrl);

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

      feats = feats.Where(f => f.Name != null).ToList();

      feats.AddRange(feats.Where(f => !String.IsNullOrEmpty(f.ParentName))
                          .GroupBy(f => f.ParentName)
                          .Select(g => new Feat { Name = g.Key, IsParentHeader = true }));

      DataFileStore.StoreToDatabase<Feat>(dbFeats => {
        List<Feat> newFeats = new List<Feat>();

        foreach(Feat feat in feats) {
          Feat dbFeat = dbFeats.SingleOrDefault(f => f.ParentName == feat.ParentName
                                                  && f.Name       == feat.Name);

          if (dbFeat != null) {
            dbFeat.Description = feat.Description;
            dbFeat.Icon        = feat.Icon;
            dbFeat.Locks       = feat.Locks;
            dbFeat.NeedsAll    = feat.NeedsAll;
            dbFeat.NeedsOne    = feat.NeedsOne;
            dbFeat.Tags        = feat.Tags;
          }
          else newFeats.Add(feat);
        }

        return newFeats;
      });

      return feats.Cast<T>().ToList();
    }

    #endregion

    #region Private Methods

    private static List<FeatNeed> buildNeedsList(string value) {
      string[] needs = value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

      return needs.Select(need => {
        string[] parts = need.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

        return new FeatNeed {
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

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
  public class SpellFileLoader : DataFileLoaderBase, IDataFileLoader {

    #region Private Fields

    private const string Filename = "SpellFile.txt";

    private const string FileUrl = "https://raw.githubusercontent.com/stricq/DDOCharPlannerV4/master/DataFiles/SpellFile.txt";

    private const string ImageUrl = "https://raw.githubusercontent.com/stricq/DDOCharPlannerV4/master/Graphics/Spells";

    #endregion Private Fields

    #region IDataFileLoader Implementation

    public Type LoaderType => typeof(Spell);

    public string LoaderName => "Spells";

    public async Task<List<T>> LoadFromDataFileAsync<T>(string FilePath, string ImagePath) {
      HttpClient client = new HttpClient();

      string file = Path.Combine(FilePath, Filename);

      await VerifyAndDownloadAsync(client, file, FileUrl);

      StreamReader stream = new StreamReader(file);

      List<Spell> spells = await ReadDataFileAsync<Spell>(stream, (spell, property, value) => {
        if (spell.CasterLevels == null) spell.CasterLevels = new List<SpellCasterLevel>();

        switch(property) {
          case "SPELLNAME": {
            spell.Name = value;

            break;
          }
          case "DESCRIPTION": {
            spell.Description = value;

            break;
          }
          case "CLASSLIST":
          case "LEVEL": {
            buildSpellCasterList(property, value, spell);

            break;
          }
          case "SCHOOL": {
            spell.School = value;

            break;
          }
          case "COST": {
            spell.Costs = ToStringList(value);

            break;
          }
          case "COMPONENTS": {
            spell.Components = ToStringList(value);

            break;
          }
          case "META": {
            spell.MetaMagics = ToStringList(value);

            break;
          }
          case "RANGE": {
            spell.Range = ToNullString(value);

            break;
          }
          case "TARGET": {
            spell.Targets = ToStringList(value);

            break;
          }
          case "DURATION": {
            spell.Duration = ToNullString(value);

            break;
          }
          case "COOLDOWN": {
            spell.CoolDowns = ToStringList(value);

            break;
          }
          case "SAVING": {
            spell.SavingThrow = ToNullString(value);

            break;
          }
          case "RESIST": {
            spell.Resistance = ToNullString(value);

            break;
          }
          case "ICON": {
            spell.Icon = value;

            break;
          }
          case "RARE": {
            spell.CanBeBought = value == "0";

            break;
          }
          default: {
            Console.WriteLine($"Encountered unexpected property '{property}' while reading {Filename}");

            break;
          }
        }
      });

      stream.Close();

      await spells.Where(spell => !String.IsNullOrEmpty(spell.Icon)).ForEachAsync(spell => {
        string path = Path.Combine(ImagePath, "Spells", spell.IconFilename);

        string url = $"{ImageUrl}/{spell.IconFilename}";
        //
        // ReSharper disable once AccessToDisposedClosure - everything is awaited
        //
        return VerifyAndDownloadAsync(client, path, url).ContinueWith(task => {
          if (task.IsCompleted && !task.Result) spell.Icon = "NoImage";
        });
      });

      client.Dispose();

      return spells.Cast<T>().ToList();
    }

    #endregion IDataFileLoader Implementation

    #region Private Methods

    private static void buildSpellCasterList(string property, string value, Spell spell) {
      if (String.IsNullOrEmpty(value)) return;

      List<string> parts = ToStringList(value);

      if (!spell.CasterLevels.Any()) for(int i = 0; i < parts.Count; ++i) spell.CasterLevels.Add(new SpellCasterLevel());

      spell.CasterLevels = spell.CasterLevels.Zip(parts, (sp, item) => {
        switch(property) {
          case "CLASSLIST": {
            sp.ClassName = Enumeration.FromDisplayName<ClassName>(item);

            break;
          }
          case "LEVEL": {
            sp.CasterLevel = Int32.Parse(item);

            break;
          }
        }

        return sp;
      }).ToList();
    }

    #endregion Private Methods

  }

}

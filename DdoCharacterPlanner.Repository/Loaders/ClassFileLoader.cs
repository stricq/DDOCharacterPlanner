using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using DdoCharacterPlanner.Domain.Contracts;
using DdoCharacterPlanner.Domain.Enumerations;
using DdoCharacterPlanner.Domain.Models.PlayerCharacter;

using STR.Common.Contracts;


namespace DdoCharacterPlanner.Repository.Loaders {

  [Export(typeof(IDataFileLoader))]
  public class ClassFileLoader : DataFileLoaderBase, IDataFileLoader {

    #region Private Fields

    private const string Filename = "ClassFile.txt";

    private const string FileUrl = "https://raw.githubusercontent.com/DDOCharPlanner/DDOCharPlannerV4/master/DataFiles/ClassFile.txt";

    #endregion Private Fields

    #region IDataFileLoader Members

    public Type LoaderType => typeof(Class);

    public async Task<List<T>> LoadFromDataFileAsync<T>(string FilePath, IDataFileStore DataFileStore) {
      string file = Path.Combine(FilePath, Filename);

      await VerifyAndDownloadAsync(file, FileUrl);

      StreamReader stream = new StreamReader(file);

      List<Class> classes = await ReadDataFileAsync<Class>(stream, (@class, property, value) => {
        switch (property) {
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
        }
      });

      classes = classes.Where(c => c.Name != null).ToList();

      DataFileStore.StoreToDatabase<Class>(dbClasses => {
        List<Class> newClasses = new List<Class>();

        foreach(Class @class in classes) {
          Class dbClass = dbClasses.SingleOrDefault(r => r.Name == @class.Name);

          if (dbClass != null) {
            dbClass.Description     = @class.Description;
            dbClass.HitDie          = @class.HitDie;
            dbClass.SkillPoints     = @class.SkillPoints;
            dbClass.BaseAttackBonus = @class.BaseAttackBonus;
            dbClass.FortitudeSave   = @class.FortitudeSave;
            dbClass.ReflexSave      = @class.ReflexSave;
            dbClass.WillSave        = @class.WillSave;
            dbClass.SpellPoints     = @class.SpellPoints;
            dbClass.Alignments      = @class.Alignments;
          }
          else newClasses.Add(@class);
        }

        return newClasses;
      });

      return classes.Cast<T>().ToList();
    }

    #endregion IDataFileLoader Members

  }

}

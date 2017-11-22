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
  public class SkillFileLoader : DataFileLoaderBase, IDataFileLoader {

    #region Private Fields

    private const string Filename = "SkillFile.txt";

    private const string FileUrl = "https://raw.githubusercontent.com/DDOCharPlanner/DDOCharPlannerV4/master/DataFiles/SkillFile.txt";

    #endregion Private Fields

    #region IDataFileLoader Members

    public Type LoaderType => typeof(Skill);

    public async Task<List<T>> LoadFromDataFileAsync<T>(string FilePath, IDataFileStore DataFileStore) {
      string file = Path.Combine(FilePath, Filename);

      await VerifyAndDownloadAsync(file, FileUrl);

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
            skill.PrimaryClasses = ToStringList(value).Select(Enumeration.FromDisplayName<ClassName>).ToList();

            break;
          }
          case "CROSS": {
            skill.CrossClasses = ToStringList(value).Select(Enumeration.FromDisplayName<ClassName>).ToList();

            break;
          }
          default: {
            Console.WriteLine($"Encountered unexpected property '{property}' while reading {Filename}");

            break;
          }
        }
      });

      skills = skills.Where(s => s.Name != null).ToList();

      DataFileStore.StoreToDatabase<Skill>(dbSkills => {
        List<Skill> newSkills = new List<Skill>();

        foreach(Skill skill in skills) {
          Skill dbSkill = dbSkills.FirstOrDefault(r => r.Name == skill.Name);

          if (dbSkill != null) {
            dbSkill.Description    = skill.Description;
            dbSkill.Icon           = skill.Icon;
            dbSkill.KeyAbility     = skill.KeyAbility;
            dbSkill.PrimaryClasses = skill.PrimaryClasses;
            dbSkill.CrossClasses   = skill.CrossClasses;
          }
          else newSkills.Add(skill);
        }

        return newSkills;
      });

      return skills.Cast<T>().ToList();
    }

    #endregion IDataFileLoader Members

  }

}

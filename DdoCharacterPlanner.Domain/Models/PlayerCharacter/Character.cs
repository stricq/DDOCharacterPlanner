using System.Collections.Generic;
using System.Linq;

using DdoCharacterPlanner.Domain.Enumerations;
using DdoCharacterPlanner.Domain.Extensions;
using DdoCharacterPlanner.Domain.Models.CommonData;


namespace DdoCharacterPlanner.Domain.Models.PlayerCharacter {

  public class Character {

    #region Private Fields

    #endregion Private Fields

    #region Properties

    public string FirstName { get; set; }

    public string SurName { get; set; }

    public Gender Gender { get; set; }

    public Alignment Alignment { get; set; }

    public Race Race { get; set; }

    public List<PastLife> PastLives { get; set; }

    public List<Level> Levels { get; set; }

    public bool Has1750Favor { get; set; }

    #endregion Properties

    #region Constructor

    public Character() {
      PastLives = new List<PastLife>();

      Levels = new List<Level>();
    }

    #endregion Constructor

    #region Domain Properties

    public static int MinLevel { get; } = 1;

    public static int MaxLevel { get; } = 30;

    public Level this[int Level] => Levels.Single(level => level.CharacterLevel == Level);

    public string Name => $"{FirstName} {SurName}".TrimEnd();

    #endregion Domain Properties

    #region Domain Methods

    public Character Clear(List<Race> Races, List<Class> Classes) {
      Race = Races.FindRace(RaceName.Human);

      Class defaultClass = Classes.FindClass(ClassName.Fighter);

      for(int i = MinLevel; i <= MaxLevel; i++) {
        Levels.Add(new Level {
          CharacterLevel = i,
          Class          = defaultClass
        });
      }

      return this;
    }

    #endregion Domain Methods

  }

}

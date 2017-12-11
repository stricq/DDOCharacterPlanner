using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;

using AutoMapper;

using DdoCharacterPlanner.Constants;

using DdoCharacterPlanner.Domain.Contracts;
using DdoCharacterPlanner.Domain.Enumerations;
using DdoCharacterPlanner.Domain.Models;
using DdoCharacterPlanner.Domain.Models.CommonData;

using DdoCharacterPlanner.ViewEntities;
using DdoCharacterPlanner.ViewEntities.Content.CommonData;

using STR.Common.Contracts;


namespace DdoCharacterPlanner.Mappings {

  [Export(typeof(IAutoMapperConfiguration))]
  public class ViewEntityMappingConfiguration : IAutoMapperConfiguration {

    #region IAutoMapperConfiguration Implementation

    public void RegisterMappings(IMapperConfigurationExpression config) {

      config.CreateMap<Settings, SettingsViewEntity>().ForMember(dest => dest.AreSettingsChanged, opt => opt.Ignore())
                                                      .ForMember(dest => dest.MainWindowState,    opt => opt.ResolveUsing(src => src.Maximized ? WindowState.Maximized : WindowState.Normal));

      config.CreateMap<SettingsViewEntity, Settings>().ForMember(dest => dest.Maximized, opt => opt.ResolveUsing(src => src.MainWindowState == WindowState.Maximized));

      config.CreateMap<Race, RaceViewEntity>().ForMember(dest => dest.Name,               opt => opt.ResolveUsing(src => src.Name.DisplayName))
                                              .ForMember(dest => dest.BaseAbilities,      opt => opt.ResolveUsing(src => buildAbilitiesCollection(src.BaseAbilities)))
                                              .ForMember(dest => dest.Description,        opt => opt.ResolveUsing(src => $"{RichTextFormat.Prefix}{src.Description}{RichTextFormat.Suffix}"))
                                              .ForMember(dest => dest.IsChecked,          opt => opt.Ignore())
                                              .ForMember(dest => dest.Iconic,             opt => opt.Ignore())
                                              .ForMember(dest => dest.MaleIconFilename,   opt => opt.Ignore())
                                              .ForMember(dest => dest.FemaleIconFilename, opt => opt.Ignore());

      config.CreateMap<Class, ClassViewEntity>().ForMember(dest => dest.Name,           opt => opt.ResolveUsing(src => src.Name.DisplayName))
                                                .ForMember(dest => dest.StartingSphere, opt => opt.ResolveUsing(src => src.StartingSphere.DisplayName))
                                                .ForMember(dest => dest.Description,    opt => opt.ResolveUsing(src => $"{RichTextFormat.Prefix}{src.Description}{RichTextFormat.Suffix}"))
                                                .ForMember(dest => dest.LevelStats,     opt => opt.ResolveUsing(buildClassLevelStats))
                                                .ForMember(dest => dest.IsChecked,      opt => opt.Ignore())
                                                .ForMember(dest => dest.IconFilename,   opt => opt.Ignore());

      config.CreateMap<Skill, SkillViewEntity>().ForMember(dest => dest.KeyAbility,   opt => opt.ResolveUsing(src => src.KeyAbility.DisplayName))
                                                .ForMember(dest => dest.Description,  opt => opt.ResolveUsing(src => $"{RichTextFormat.Prefix}{src.Description}{RichTextFormat.Suffix}"))
                                                .ForMember(dest => dest.IsChecked,    opt => opt.Ignore())
                                                .ForMember(dest => dest.IconFilename, opt => opt.Ignore());
    }

    #endregion IAutoMapperConfiguration Implementation

    #region Private Methods

    private static ObservableCollection<AbilityViewEntity> buildAbilitiesCollection(int[] abilities) {
      return new ObservableCollection<AbilityViewEntity>(Enumeration.GetAll<Ability>().Select(a => {
        int points = abilities[a.Value];

        return new AbilityViewEntity {
          Ability  = a.DisplayName,
          ShortName    = a.ShortName,
          Points   = points,
          Modifier = a.Modifier(points)
        };
      }));
    }

    private static ObservableCollection<ClassLevelStatsViewEntity> buildClassLevelStats(Class @class) {
      ObservableCollection<ClassLevelStatsViewEntity> entities = new ObservableCollection<ClassLevelStatsViewEntity>();

      for(int i = 0; i < @class.BaseAttackBonuses.Length; ++i) {
        entities.Add(new ClassLevelStatsViewEntity {
          Level           = i + 1,
          BaseAttackBonus = @class.BaseAttackBonuses[i],
          FortitudeSave   = @class.FortitudeSaves[i],
          ReflexSave      = @class.ReflexSaves[i],
          WillSave        = @class.WillSaves[i],
          SpellPoints     = @class.SpellPoints[i]
        });
      }

      return entities;
    }

    #endregion Private Methods

  }

}

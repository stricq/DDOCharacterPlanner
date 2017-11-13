using System.ComponentModel.Composition;
using System.Windows;

using AutoMapper;

using DdoCharacterPlanner.Domain.Contracts;
using DdoCharacterPlanner.Domain.Models;

using DdoCharacterPlanner.ViewEntities;


namespace DdoCharacterPlanner.Mappings {

  [Export(typeof(IAutoMapperConfiguration))]
  public class ViewEntityMappingConfiguration : IAutoMapperConfiguration {

    #region IAutoMapperConfiguration Implementation

    public void RegisterMappings(IMapperConfigurationExpression config) {

      config.CreateMap<DomainSettings, SettingsViewEntity>().ForMember(dest => dest.AreSettingsChanged, opt => opt.Ignore())
                                                            .ForMember(dest => dest.MainWindowState,    opt => opt.ResolveUsing(src => src.Maximized ? WindowState.Maximized : WindowState.Normal));

      config.CreateMap<SettingsViewEntity, DomainSettings>().ForMember(dest => dest.Maximized, opt => opt.ResolveUsing(src => src.MainWindowState == WindowState.Maximized));
    }

    #endregion IAutoMapperConfiguration Implementation

  }

}

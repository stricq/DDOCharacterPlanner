using AutoMapper;


namespace DdoCharacterPlanner.Domain.Contracts {

  public interface IAutoMapperConfiguration {

    void RegisterMappings(IMapperConfigurationExpression config);

  }

}

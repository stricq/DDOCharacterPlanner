using System.Threading.Tasks;

using DdoCharacterPlanner.Domain.Models;


namespace DdoCharacterPlanner.Domain.Contracts {

  public interface ISettingsRepository {

    Task<DomainSettings> LoadSettingsAsync();

    Task SaveSettingsAsync(DomainSettings Settings);

  }

}

using System.Threading.Tasks;

using DdoCharacterPlanner.Domain.Models;


namespace DdoCharacterPlanner.Domain.Contracts {

  public interface ISettingsRepository {

    Task<Settings> LoadSettingsAsync();

    Task SaveSettingsAsync(Settings Settings);

  }

}

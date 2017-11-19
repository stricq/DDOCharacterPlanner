using DdoCharacterPlanner.Domain.Models;

using STR.Common.Messages;


namespace DdoCharacterPlanner.Messages.Application {

  public class AppLoadedMessage : ApplicationLoadedMessage {

    public Settings Settings { get; set; }

  }

}

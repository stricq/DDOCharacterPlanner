using DdoCharacterPlanner.Domain.Models;

using STR.Common.Messages;


namespace DdoCharacterPlanner.Messages.Application {

  public class AppLoadedMessage : ApplicationLoadedMessage {

    public DomainSettings Settings { get; set; }

  }

}

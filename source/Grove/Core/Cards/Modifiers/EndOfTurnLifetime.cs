namespace Grove.Core.Cards.Modifiers
{
  using Grove.Infrastructure;
  using Grove.Core.Messages;

  public class EndOfTurnLifetime : Lifetime, IReceive<EndOfTurn>
  {    
    public void Receive(EndOfTurn message)
    {
      End();
    }
  }
}
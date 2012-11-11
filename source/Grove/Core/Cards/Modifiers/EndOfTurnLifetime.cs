namespace Grove.Core.Details.Cards.Modifiers
{
  using Infrastructure;
  using Messages;

  public class EndOfTurnLifetime : Lifetime, IReceive<EndOfTurn>
  {    
    public void Receive(EndOfTurn message)
    {
      End();
    }
  }
}
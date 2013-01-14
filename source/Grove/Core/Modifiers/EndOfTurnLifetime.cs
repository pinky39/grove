namespace Grove.Core.Modifiers
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
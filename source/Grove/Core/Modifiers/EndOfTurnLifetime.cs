namespace Grove.Modifiers
{
  using Grove.Events;
  using Grove.Infrastructure;

  public class EndOfTurnLifetime : Lifetime, IReceive<EndOfTurnEvent>
  {
    public void Receive(EndOfTurnEvent message)
    {
      End();
    }
  }
}
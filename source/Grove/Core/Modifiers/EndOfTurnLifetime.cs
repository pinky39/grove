namespace Grove.Modifiers
{
  using Grove.Events;
  using Grove.Infrastructure;

  public class EndOfTurnLifetime : Lifetime, IReceive<EndOfTurn>
  {
    public void Receive(EndOfTurn message)
    {
      End();
    }
  }
}
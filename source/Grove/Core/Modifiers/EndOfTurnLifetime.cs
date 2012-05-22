namespace Grove.Core.Modifiers
{
  using Infrastructure;
  using Messages;

  public class EndOfTurnLifetime : Lifetime, IReceive<EndOfTurn>
  {
    private EndOfTurnLifetime() {}

    public EndOfTurnLifetime(Modifier modifier, ChangeTracker changeTracker) : base(modifier, changeTracker) { }

    public void Receive(EndOfTurn message)
    {
      End();
    }
  }
}
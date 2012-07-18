namespace Grove.Core.Details.Cards.Modifiers
{
  using Infrastructure;
  using Messages;

  public class EndOfTurnLifetime : Lifetime, IReceive<EndOfTurn>
  {
    private EndOfTurnLifetime() {}

    public EndOfTurnLifetime(ChangeTracker changeTracker) : base(changeTracker) {}

    public void Receive(EndOfTurn message)
    {
      End();
    }
  }
}
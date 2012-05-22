namespace Grove.Core.Modifiers
{
  using Infrastructure;
  using Messages;

  public class PermanentLifetime : Lifetime, IReceive<CardChangedZone>
  {
    private PermanentLifetime()
    {      
    }

    public PermanentLifetime(Modifier modifier, ChangeTracker changeTracker) : base(modifier, changeTracker) { }

    public void Receive(CardChangedZone message)
    {
      if (message.Card != ModifierTarget)
        return;

      if (message.FromBattlefield)
        End();
    }
  }
}
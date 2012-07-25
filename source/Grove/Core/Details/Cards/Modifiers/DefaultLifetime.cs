namespace Grove.Core.Details.Cards.Modifiers
{
  using Infrastructure;
  using Messages;
  using Targeting;

  public class DefaultLifetime : Lifetime, IReceive<CardChangedZone>
  {
    private readonly Target _target;
    private DefaultLifetime() {}

    public DefaultLifetime(Target target, ChangeTracker changeTracker) 
      : base(changeTracker)
    {
      _target = target;
    }

    public void Receive(CardChangedZone message)
    {
      if (message.Card != _target.Card())
        return;

      if (message.FromBattlefield)
        End();
    }
  }
}
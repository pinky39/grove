namespace Grove.Core.Details.Cards.Modifiers
{
  using Infrastructure;
  using Messages;
  using Targeting;

  public class DefaultLifetime : Lifetime, IReceive<CardChangedZone>
  {
    private readonly ITarget _target;
    private DefaultLifetime() {}

    public DefaultLifetime(ITarget target, ChangeTracker changeTracker) 
      : base(changeTracker)
    {
      _target = target;
    }

    public void Receive(CardChangedZone message)
    {
      if (message.Card != _target)
        return;

      if (message.FromBattlefield)
        End();
    }
  }
}
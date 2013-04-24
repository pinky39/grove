namespace Grove.Core.Modifiers
{
  using Infrastructure;
  using Messages;
  using Targeting;

  public class DefaultLifetime : Lifetime, IReceive<ZoneChanged>
  {
    private ITarget _target;

    public void Receive(ZoneChanged message)
    {
      if (message.Card != _target)
        return;

      if (message.FromBattlefield)
        End();
    }

    public override void Initialize(Modifier modifier, Game game)
    {
      base.Initialize(modifier, game);

      _target = modifier.Target;
    }
  }
}
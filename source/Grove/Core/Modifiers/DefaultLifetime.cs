namespace Grove.Core.Modifiers
{
  using Infrastructure;
  using Messages;
  using Targeting;

  public class DefaultLifetime : Lifetime, IReceive<ZoneChanged>
  {
    private readonly ITarget _target;

    public DefaultLifetime(ITarget target)
    {
      _target = target;
    }    

    public void Receive(ZoneChanged message)
    {
      if (message.Card != _target)
        return;

      if (message.FromBattlefield)
        End();
    }
  }
}
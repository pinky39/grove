namespace Grove.Gameplay.Modifiers
{
  using System;
  using Card;
  using Infrastructure;
  using Messages;

  public class PermanentGetsUntapedLifetime : Lifetime,
    IReceive<PermanentGetsUntapped>, IReceive<ZoneChanged>
  {
    private readonly Func<Lifetime, Card> _permanent;

    private PermanentGetsUntapedLifetime() {}

    public PermanentGetsUntapedLifetime(Func<Lifetime, Card> permanent)
    {
      _permanent = permanent;
    }

    public void Receive(PermanentGetsUntapped message)
    {
      if (message.Permanent == _permanent(this))
      {
        End();
      }
    }

    public void Receive(ZoneChanged message)
    {
      if (message.Card == _permanent(this) && message.FromBattlefield)
      {
        End();
      }
    }
  }
}
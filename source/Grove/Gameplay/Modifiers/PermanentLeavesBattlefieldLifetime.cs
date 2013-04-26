namespace Grove.Gameplay.Modifiers
{
  using System;
  using Card;
  using Infrastructure;
  using Messages;

  public class PermanentLeavesBattlefieldLifetime : Lifetime, IReceive<ZoneChanged>
  {
    private readonly Func<Lifetime, Card> _selector;

    private PermanentLeavesBattlefieldLifetime() {}

    public PermanentLeavesBattlefieldLifetime(Func<Lifetime, Card> selector)
    {
      _selector = selector;
    }

    public void Receive(ZoneChanged message)
    {
      if (message.Card == _selector(this) && message.FromBattlefield)
      {
        End();
      }
    }
  }
}
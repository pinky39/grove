namespace Grove.Modifiers
{
  using System;
  using Grove.Events;
  using Grove.Infrastructure;

  public class PermanentLeavesBattlefieldLifetime : Lifetime, IReceive<ZoneChangedEvent>
  {
    private readonly Func<Lifetime, Card> _selector;

    private PermanentLeavesBattlefieldLifetime() {}

    public PermanentLeavesBattlefieldLifetime(Func<Lifetime, Card> selector)
    {
      _selector = selector;
    }

    public void Receive(ZoneChangedEvent message)
    {
      if (message.Card == _selector(this) && message.FromBattlefield)
      {
        End();
      }
    }
  }
}
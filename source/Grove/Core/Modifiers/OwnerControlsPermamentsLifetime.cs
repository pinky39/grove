namespace Grove.Modifiers
{
  using System;
  using System.Linq;
  using Events;
  using Infrastructure;

  public class OwnerControlsPermamentsLifetime : Lifetime, IReceive<ZoneChangedEvent>
  {
    private readonly Func<Card, bool> _selector;

    private OwnerControlsPermamentsLifetime() {}

    public OwnerControlsPermamentsLifetime(Func<Card, bool> selector)
    {
      _selector = selector;
    }

    public void Receive(ZoneChangedEvent message)
    {
      if (_selector(message.Card) && !(Modifier.SourceCard.Controller.Battlefield.Any(_selector)))
      {
        End();
      }
    }
  }
}
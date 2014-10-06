namespace Grove.Modifiers
{
  using System;
  using System.Linq;
  using Events;
  using Infrastructure;

  public class OwnerHasCardsInExile : Lifetime, IReceive<ZoneChangedEvent>
  {
    private readonly Func<Card, bool> _selector;

    private OwnerHasCardsInExile() { }

    public OwnerHasCardsInExile(Func<Card, bool> selector)
    {
      _selector = selector ?? delegate { return true; };
    }

    public void Receive(ZoneChangedEvent message)
    {
      if (message.From != Zone.Exile)
        return;

      if (_selector(message.Card) && !(Modifier.SourceCard.Controller.Exile.Any(_selector)))
      {
        End();
      }
    }
  }
}

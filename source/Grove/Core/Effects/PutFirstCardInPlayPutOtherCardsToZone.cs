namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;

  public class PutFirstCardInPlayPutOtherCardsToZone : Effect
  {
    private readonly Func<Card, bool> _filter;
    private readonly Zone _toZone;

    private PutFirstCardInPlayPutOtherCardsToZone() {}

    public PutFirstCardInPlayPutOtherCardsToZone(Zone toZone, Func<Card, bool> filter = null)
    {
      _toZone = toZone;
      _filter = filter ?? delegate { return true; };
    }

    protected override void ResolveEffect()
    {
      var toOtherZone = new List<Card>();

      foreach (var card in Controller.Library)
      {
        if (_filter(card))
        {
          card.PutToBattlefield();

          break;
        }

        toOtherZone.Add(card);
      }

      foreach (var card in toOtherZone)
      {
        switch (_toZone)
        {
          case Zone.Graveyard:
            card.PutToGraveyard();
            break;

          case Zone.Library:
            card.Owner.PutOnBottomOfLibrary(card);
            break;

          default:
            throw new NotSupportedException("Zone is not supported: " + _toZone);
        }
      }
    }
  }
}
namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;

  public class PutFirstCardInPlayPutOtherCardsInZone : Effect
  {
    private readonly Func<Card, bool> _filter;
    private readonly Zone _zone;

    private PutFirstCardInPlayPutOtherCardsInZone() {}

    public PutFirstCardInPlayPutOtherCardsInZone(Zone zone, Func<Card, bool> filter = null)
    {
      _zone = zone;
      _filter = filter ?? delegate { return true; };
    }

    protected override void ResolveEffect()
    {
      var toGraveyard = new List<Card>();
      
      foreach (var card in Controller.Library)
      {
        if (_filter(card))
        {
          card.PutToBattlefield();
        
          break;                    
        }
        
        toGraveyard.Add(card);
      }

      foreach (var card in toGraveyard)
      {
        switch (_zone)
        {
          case Zone.Graveyard:
            card.PutToGraveyard();
            break;

          case Zone.Library:
            card.Owner.PutOnBottomOfLibrary(card);
            break;

          default:
            throw new NotSupportedException("Zone is not supported: " + _zone);
        }        
      }
    }
  }
}
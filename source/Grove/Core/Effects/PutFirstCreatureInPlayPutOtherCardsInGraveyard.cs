namespace Grove.Effects
{
  using System.Collections.Generic;

  public class PutFirstCreatureInPlayPutOtherCardsInGraveyard : Effect
  {
    protected override void ResolveEffect()
    {
      var toGraveyard = new List<Card>();
      Card creature = null;
      
      foreach (var card in Controller.Library)
      {
        if (card.Is().Creature)
        {
          creature = card;          
          break;                    
        }
        
        toGraveyard.Add(card);
      }

      if (creature != null)
        creature.PutToBattlefield();

      foreach (var card in toGraveyard)
      {
        card.PutToGraveyard();
      }
    }
  }
}
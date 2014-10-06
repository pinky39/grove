namespace Grove.AI
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

  public static class CardPicker
  {
    public static ChosenCards ChooseBestCards(Player controller, IEnumerable<Card> candidates, int count, bool aurasNeedTarget)
    {
       var ordered = candidates
        .OrderBy(x => -x.Score)        
        .ToList();

      var chosenCards = new List<Card>();

      int currentCount = 0;

      foreach (var card in ordered)
      {
        if (currentCount >= count)
          break;
        
        // not an aura just choose the card
        if (!card.Is().Aura)
        {
          chosenCards.Add(card);
          currentCount++;
          continue;  
        }
                        
        // find something to attach aura to
        // or skip to next best card
        var bestAuraTarget = card.Controller.Battlefield
          .Where(target => card.CanTarget(target) && card.IsGoodTarget(target, controller))
          .OrderBy(x => -x.Score)
          .FirstOrDefault();

        if (bestAuraTarget != null)
        {
          chosenCards.Add(card);

          if (aurasNeedTarget)
          {
            chosenCards.Add(bestAuraTarget);
          }

          currentCount++;
        }
      }

      return new ChosenCards(chosenCards);
    }    
  }
}
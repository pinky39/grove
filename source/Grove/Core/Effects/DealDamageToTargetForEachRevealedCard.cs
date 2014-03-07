namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

  public class DealDamageToTargetForEachRevealedCard : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly Func<Card, bool> _filter;

    private DealDamageToTargetForEachRevealedCard()
    {
    }

    public DealDamageToTargetForEachRevealedCard(Func<Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    protected override void ResolveEffect()
    {
      Enqueue(new SelectCards(Controller, p =>
        {
          p.SetValidator(_filter);
          p.Zone = Zone.Hand;
          p.MinCount = 0;
          p.Text = "Choose any number of cards in your hand.";
          p.OwningCard = Source.OwningCard;
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
        }));
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var chosenCard in results)
      {
        chosenCard.Reveal();
      }

      Source.OwningCard.DealDamageTo(
        results.Count,
        (IDamageable)Target,
        isCombat: false);
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      var cardsToReveal = Target.Is().Creature ? Target.Card().Life : int.MaxValue;
      return candidates.OrderBy(x => x.Score).Take(cardsToReveal).ToList();
    }
  }
}
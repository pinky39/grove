namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Modifiers;

  public class CreatureGetsM1M1ForEachRevealedCard : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly Func<Card, bool> _filter;

    private CreatureGetsM1M1ForEachRevealedCard()
    {
    }

    public CreatureGetsM1M1ForEachRevealedCard(Func<Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      var cardsToReveal = Target.Card().Life;
      return candidates.OrderBy(x => x.Score).Take(cardsToReveal).ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var chosenCard in results)
      {
        chosenCard.Reveal();
      }

      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      var modifier = new AddPowerAndToughness(-results.Count, -results.Count);
      Target.Card().AddModifier(modifier, p);
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
  }
}
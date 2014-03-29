namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Modifiers;

  public class CreatureGetsPwtForEachRevealedCard : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly Func<Card, bool> _filter;
    private readonly int _power;
    private readonly int _toughness;

    private CreatureGetsPwtForEachRevealedCard() {}

    public CreatureGetsPwtForEachRevealedCard(int power, int toughness,
      Func<Card, bool> filter = null)
    {
      _power = power;
      _toughness = toughness;
      _filter = filter ?? delegate { return true; };
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      if (_toughness < 0)
      {
        var targetLife = (double) Target.Card().Life;
        var cardsToReveal = (int) Math.Ceiling(targetLife/-_toughness);
        return candidates.OrderBy(x => x.Score).Take(cardsToReveal).ToList();
      }

      // all
      return candidates;
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
          X = X,
        };

      var modifier = new AddPowerAndToughness(_power*results.Count, _toughness*results.Count) {UntilEot = true};
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
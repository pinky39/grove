namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using Decisions;

  public class GainLifeForEachRevealedCard : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly Func<Card, bool> _filter;
    private readonly int _amount;
    private Player _you;

    protected override void Initialize()
    {
      _you = Controller;
    }
    
    private GainLifeForEachRevealedCard() {}

    public GainLifeForEachRevealedCard(Func<Card, bool> filter, int amount)
    {
      _filter = filter;
      _amount = amount;
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates;
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var chosenCard in results)
      {
        chosenCard.Reveal();
      }

      _you.Life += _amount * results.Count;
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
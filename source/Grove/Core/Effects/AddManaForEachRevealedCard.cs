namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using Decisions;

  public class AddManaForEachRevealedCard : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly Func<Card, bool> _filter;
    private readonly int _amount;

    private AddManaForEachRevealedCard()
    {
      
    }

    public AddManaForEachRevealedCard(Func<Card, bool> filter, int amount)
    {
      _filter = filter;
      _amount = amount;
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
      
      var manaAmount = (results.Count*_amount).Colorless();
      
      Controller.AddManaToManaPool(manaAmount);
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates;      
    }
  }
}
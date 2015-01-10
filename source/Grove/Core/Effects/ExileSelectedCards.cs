namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Modifiers;

  public class ExileSelectedCards : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly Value _amount;
    private readonly Zone _from;
    private readonly Func<Card, bool> _filter;

    private ExileSelectedCards() {}

    public ExileSelectedCards(Value amount, Zone from, Func<Card, bool> filter = null)
    {
      _amount = amount;
      _from = from;
      _filter = filter ?? delegate { return true; };
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      var maxCount = _amount.GetValue(X);

      return candidates
        .OrderBy(x => x.Score)
        .Take(maxCount)
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var chosenCard in results)
      {
        chosenCard.Exile();
      }
    }

    protected override void ResolveEffect()
    {
      var count = _amount.GetValue(X);

      Enqueue(new SelectCards(Controller,
        p =>
        {
          p.SetValidator(_filter);
          p.Zone = _from;
          p.MinCount = count;
          p.MaxCount = count;
          p.Text = "Select cards to exile.";
          p.OwningCard = Source.OwningCard;
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
        }
        ));
    }
  }
}

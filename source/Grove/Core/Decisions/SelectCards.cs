namespace Grove.Core.Decisions
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Results;
  using Targeting;
  using Zones;

  public abstract class SelectCards : Decision<ChosenCards>
  {
    public int MaxCount;
    public int MinCount;
    public IProcessDecisionResults<ChosenCards> ProcessDecisionResults;
    public string Text;
    public Func<Card, bool> Validator;    
    public Zone Zone;
    private List<Card> _validCards;

    protected SelectCards()
    {
      Result = ChosenCards.None;
    }

    protected List<Card> ValidCards
    {
      get
      {
        return
          _validCards ?? (_validCards =
            GenerateTargets((zone, owner) => owner == Controller && zone == Zone)
              .Where(x => x.IsCard())
              .Select(x => x.Card())
              .Where(x => Validator(x))
              .ToList());
      }
    }

    protected override bool ShouldExecuteQuery
    {
      get
      {
        var count = ValidCards.Count;
        return count > MinCount;
      }
    }

    public override void ProcessResults()
    {
      if (Result.None())
      {
        foreach (var card in ValidCards)
        {
          ProcessCard(card);
        }
      }
      else
      {
        foreach (var chosenCard in Result)
        {
          ProcessCard(chosenCard);
        }
      }

      if (ProcessDecisionResults != null)
        ProcessDecisionResults.ProcessResults(Result);

      ResultProcessed();
    }

    protected ChosenCards GetTargets(bool descending)
    {
      return GenerateTargets((zone, owner) => owner == Controller && Zone == zone)
        .Where(x => x.IsCard())
        .Select(x => x.Card())
        .Where(Validator)
        .OrderByDescending(x => descending ? x.Score : -x.Score)
        .Take(MaxCount)
        .ToList();
    }

    protected abstract void ProcessCard(Card chosenCard);
    protected virtual void ResultProcessed() {}
  }
}
namespace Grove.Core.Decisions
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Infrastructure;
  using Grove.Core.Targeting;
  using Results;

  public abstract class SelectCards : Decision<ChosenCards>
  {
    public int MaxCount;
    public int MinCount;
    public IProcessDecisionResults<ChosenCards> ProcessDecisionResults;
    public string Text;
    public Func<Card, bool> Validator;
    private List<Card> _validCards;

    protected List<Card> ValidCards
    {
      get
      {
        return
          _validCards ?? (_validCards =
            Game.GenerateTargets()
              .Where(x => x.IsCard())
              .Select(x => x.Card())
              .Where(x => x.Controller == Controller && Validator(x))
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
        ProcessDecisionResults.ResultProcessed(Result);
      
      ResultProcessed();
    }

    protected abstract void ProcessCard(Card chosenCard);
    protected virtual void ResultProcessed() {}
  }
}
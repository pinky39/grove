namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Decisions;
  using Infrastructure;

  public class DiscardCardToDrawCard : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    protected override void ResolveEffect()
    {
      Enqueue(new SelectCards(Controller, p =>
      {
        p.SetValidator(delegate { return true; });
        p.Zone = Zone.Hand;
        p.MinCount = 0;
        p.MaxCount = 1;
        p.Text = "Select a card to discard.";
//        p.Instructions = string.Format("(Press Enter to sacrifice {0}.)", Source.OwningCard.Name);
        p.ProcessDecisionResults = this;
        p.ChooseDecisionResults = this;
        p.OwningCard = Source.OwningCard;
      }));
    }

    public void ProcessResults(ChosenCards results)
    {
      if (results.None())
      {
        return;
      }

      results[0].Discard();

      Controller.DrawCard();
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates
        .OrderBy(x => ScoreCalculator.CalculateDiscardScore(x, Ai.IsSearchInProgress))
        .Take(1)
        .ToList();
    }
  }
}

namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

  public class SearchAuraAndAttachToOwningCard : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    protected override void ResolveEffect()
    {
      Controller.PeekLibrary();

      Enqueue(new SelectCards(
        Controller,
        p =>
          {
            p.MinCount = 0;
            p.MaxCount = 1;
            p.SetValidator(c => c.Is().Aura &&
              (c.Zone == Zone.Graveyard || c.Zone == Zone.Library || c.Zone == Zone.Hand));
            p.Text = "Search graveyard, hand, library for an Aura card.";
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
            p.OwningCard = Source.OwningCard;
            p.AurasNeedTarget = false;
          }));
    }

    public void ProcessResults(ChosenCards results)
    {
      if (results.Count == 0)
        return;

      var shuffleLibrary = results[0].Zone == Zone.Library;

      results[0].EnchantWithoutPayingCost(Source.OwningCard);

      if (shuffleLibrary)
      {
        Controller.ShuffleLibrary();
      }
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates
        .OrderBy(x => -x.ConvertedCost)
        .Take(1)
        .ToList();
    }
  }
}
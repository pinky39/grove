namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Decisions;

  public class ExileOwnerUnlessYouDiscardCreatureCard : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return new ChosenCards(
        candidates.OrderBy(x => -x.Score).ToList());
    }

    public void ProcessResults(ChosenCards results)
    {
      if (results.Count == 0 || results[0].Zone != Zone.Hand)
      {
        Source.OwningCard.ExileFrom(Zone.Battlefield);
        return;
      }

      results[0].Discard();
    }

    protected override void ResolveEffect()
    {
      Enqueue(new SelectCards(Controller, p =>
        {
          p.MinCount = 0;
          p.MaxCount = 1;
          p.SetValidator(card => card.Is().Creature);
          p.Zone = Zone.Hand;
          p.Text = "Select a creature to discard";
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
          p.OwningCard = Source.OwningCard;
        }));
    }
  }
}
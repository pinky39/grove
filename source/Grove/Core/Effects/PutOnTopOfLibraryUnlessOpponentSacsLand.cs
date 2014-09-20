namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Decisions;

  public class PutOnTopOfLibraryUnlessOpponentSacsLand : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    public ChosenCards ChooseResult(List<Card> candidates)
    {
      if (Controller.Opponent.Battlefield.Lands.Count() < 4)
        return new ChosenCards();

      return candidates
        .OrderBy(x => x.Score)
        .Take(1)
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      if (results.Count == 0)
        return;

      results[0].Sacrifice();

      Source.OwningCard.PutOnTopOfLibraryFrom(Zone.Battlefield);
    }

    protected override void ResolveEffect()
    {
      Enqueue(new SelectCards(Controller.Opponent, p =>
        {
          p.SetValidator(c => c.Is().Land);
          p.Zone = Zone.Battlefield;
          p.Text = "Select a land to sacrifice or press enter.";
          p.ChooseDecisionResults = this;
          p.ProcessDecisionResults = this;
          p.MinCount = 0;
          p.MaxCount = 1;
          p.OwningCard = Source.OwningCard;
        }));
    }
  }
}
namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Card;
  using Decisions;
  using Decisions.Results;
  using Zones;

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

      Source.OwningCard.PutOnTopOfLibrary();
    }

    protected override void ResolveEffect()
    {
      Enqueue<SelectCards>(Controller.Opponent, p =>
        {
          p.Validator(c => c.Is().Land);
          p.Zone = Zone.Battlefield;
          p.Text = "Sacrifice a land?";
          p.ChooseDecisionResults = this;
          p.ProcessDecisionResults = this;
          p.MinCount = 0;
          p.MaxCount = 1;
          p.OwningCard = Source.OwningCard;
        });
    }
  }
}
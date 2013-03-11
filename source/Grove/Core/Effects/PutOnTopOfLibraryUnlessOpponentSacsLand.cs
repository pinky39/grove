namespace Grove.Core.Effects
{
  using System.Linq;
  using Decisions;
  using Decisions.Results;
  using Zones;

  public class PutOnTopOfLibraryUnlessOpponentSacsLand : Effect, IProcessDecisionResults<ChosenCards>
  {
    public void ProcessResults(ChosenCards results)
    {
      if (results.Any())
      {
        Source.OwningCard.PutOnTopOfLibrary();
      }
    }

    protected override void ResolveEffect()
    {
      Enqueue<SelectCardsToSacrificeAsCost>(Players.GetOpponent(Controller), p =>
        {
          p.Validator = card => card.Is().Land;
          p.Zone = Zone.Battlefield;
          p.Text = FormatText("Select a land to sacrifice");
          p.Ai = (controller, card) => controller.Battlefield.Lands.Count() >= 4;
          p.QuestionText = "Sacrifice a land?";
          p.MinCount = 1;
          p.MaxCount = 1;
          p.ProcessDecisionResults = this;
          p.CardToPayUpkeepFor = Source.OwningCard;
          p.OwningCard = Source.OwningCard;
        });
    }
  }
}
namespace Grove.Core.Effects
{
  using System.Linq;
  using Grove.Core.Decisions;
  using Grove.Core.Decisions.Results;
  using Grove.Core.Zones;

  public class PutOnTopOfLibraryUnlessOpponentSacsLand : Effect, IProcessDecisionResults<ChosenCards>
  {
    public void ResultProcessed(ChosenCards results)
    {
      if (results.Any())
      {
        Source.OwningCard.PutOnTopOfLibrary();
      }
    }

    protected override void ResolveEffect()
    {
      Game.Enqueue<SelectCardsToSacrificeAsCost>(Players.GetOpponent(Controller), p =>
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
        });
    }
  }
}
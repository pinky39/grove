namespace Grove.Core.Cards.Effects
{
  using System.Linq;
  using Grove.Core.Decisions;
  using Grove.Core.Decisions.Results;

  public class PutOnTopOfLibraryUnlessOpponentSacsLand : Effect, IProcessDecisionResults<ChosenCards>
  {
    protected override void ResolveEffect()
    {
      Game.Enqueue<SelectCardsToSacrificeAsCost>(Players.GetOpponent(Controller), p =>
        {
          p.Validator = card => card.Is().Land;
          p.Text = "Select a land to sacrifice";
          p.Ai = (controller, card) => controller.Battlefield.Lands.Count() >= 4;
          p.QuestionText = "Sacrifice a land?";
          p.MinCount = 1;
          p.MaxCount = 1;
          p.ProcessDecisionResults = this;
          p.CardToPayUpkeepFor = Source.OwningCard;

        });
    }

    public void ResultProcessed(ChosenCards results)
    {
      if (results.Any())
      {
        Source.OwningCard.PutOnTopOfLibrary();
      }
    }
  }
}
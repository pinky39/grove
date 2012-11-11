namespace Grove.Core.Details.Cards.Effects
{
  using System;
  using System.Linq;
  using System.Windows;
  using Controllers;  
  using Controllers.Results;
  using Infrastructure;
  using Targeting;
  using Ui;

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
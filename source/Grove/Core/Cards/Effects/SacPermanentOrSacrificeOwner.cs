namespace Grove.Core.Details.Cards.Effects
{
  using System;
  using Controllers;
  using Controllers.Results;
  using Infrastructure;

  public class SacPermanentOrSacrificeOwner : Effect, IProcessDecisionResults<ChosenCards>
  {
    public Func<Player, Card, bool> ShouldPayAi = delegate { return true; };
    public string Text = "Select permanent to sacrifice";
    public Func<Card, bool> Validator = delegate { return true; };

    protected override void ResolveEffect()
    {
      Game.Enqueue<SelectCardsToSacrificeAsCost>(Controller, p =>
        {
          p.Ai = ShouldPayAi;
          p.QuestionText = FormatText("Pay upkeep cost?");
          p.Validator = Validator;
          p.MinCount = 1;
          p.MaxCount = 1;
          p.CardToPayUpkeepFor = Source.OwningCard;
          p.Text = Text;
          p.ProcessDecisionResults = this;
        });
    }

    public void ResultProcessed(ChosenCards results)
    {
      if (results.None())
      {
        Source.OwningCard.Sacrifice();
      }
    }
  }
}
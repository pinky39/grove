namespace Grove.Core.Cards.Effects
{
  using System;
  using Decisions;
  using Decisions.Results;
  using Infrastructure;
  using Zones;

  public class SacPermanentOrSacrificeOwner : Effect, IProcessDecisionResults<ChosenCards>
  {
    public Func<Player, Card, bool> ShouldPayAi = delegate { return true; };
    public string Text = "Select permanent to sacrifice";
    public Func<Card, bool> Validator = delegate { return true; };

    public void ResultProcessed(ChosenCards results)
    {
      if (results.None())
      {
        Source.OwningCard.Sacrifice();
      }
    }

    protected override void ResolveEffect()
    {
      Game.Enqueue<SelectCardsToSacrificeAsCost>(Controller, p =>
        {
          p.Ai = ShouldPayAi;
          p.QuestionText = FormatText("Pay upkeep cost?");
          p.Validator = Validator;
          p.Zone = Zone.Battlefield;
          p.MinCount = 1;
          p.MaxCount = 1;
          p.CardToPayUpkeepFor = Source.OwningCard;
          p.Text = Text;
          p.ProcessDecisionResults = this;
        });
    }
  }
}
namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class BellowingSaddlebrute : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bellowing Saddlebrute")
        .ManaCost("{3}{B}")
        .Type("Creature — Orc Warrior")
        .Text("{I}Raid{/I} — When Bellowing Saddlebrute enters the battlefield, you lose 4 life unless you attacked with a creature this turn.")
        .Power(4)
        .Toughness(5)
        .TriggeredAbility(p =>
        {
          p.Text = "When Bellowing Saddlebrute enters the battlefield, you lose 4 life unless you attacked with a creature this turn.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield)
          {
            Condition = ctx => !ctx.Turn.Events.HasActivePlayerAttackedThisTurn,
          });

          p.Effect = () => new ChangeLife(-4, yours: true);
        });
    }
  }
}

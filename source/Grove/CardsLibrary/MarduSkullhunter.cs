namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class MarduSkullhunter : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mardu Skullhunter")
        .ManaCost("{1}{B}")
        .Type("Creature — Human Warrior")
        .Text("Mardu Skullhunter enters the battlefield tapped.{EOL}{I}Raid{/I} — When Mardu Skullhunter enters the battlefield, if you attacked with a creature this turn, target opponent discards a card.")
        .Power(2)
        .Toughness(1)
        .Cast(p => { p.Effect = () => new CastPermanent(tap: true); })
        .TriggeredAbility(p =>
        {
          p.Text = "When Mardu Skullhunter enters the battlefield, if you attacked with a creature this turn, target opponent discards a card.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield)
          {
            Condition = ctx => ctx.Turn.Events.HasActivePlayerAttackedThisTurn,
          });

          p.Effect = () => new OpponentDiscardsCards(selectedCount: 1);
        });
    }
  }
}

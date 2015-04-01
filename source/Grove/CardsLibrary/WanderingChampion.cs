namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Triggers;

  public class WanderingChampion : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Wandering Champion")
        .ManaCost("{1}{W}")
        .Type("Creature — Human Monk")
        .Text("Whenever Wandering Champion deals combat damage to a player, if you control a blue or red permanent, you may discard a card. If you do, draw a card.")
        .FlavorText("\"Learn from your enemies, but do not tolerate them.\"")
        .Power(3)
        .Toughness(1)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever Wandering Champion deals combat damage to a player, if you control a blue or red permanent, you may discard a card. If you do, draw a card.";
          p.Trigger(new OnDamageDealt(dmg =>
              dmg.IsCombat && dmg.IsDealtByOwningCard && dmg.IsDealtToPlayer)
          {
            Condition = ctx => ctx.You.Battlefield.Any(x => x.HasColor(CardColor.Blue) || x.HasColor(CardColor.Red)),
          });

          p.Effect = () => new DiscardCardToDrawCard();
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}

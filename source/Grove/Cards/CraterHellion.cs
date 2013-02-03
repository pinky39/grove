namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;
  using Core.Zones;

  public class CraterHellion : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Crater Hellion")
        .ManaCost("{4}{R}{R}")
        .Type("Creature Hellion Beast")
        .Text(
          "{Echo} {4}{R}{R} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.){EOL}When Crater Hellion enters the battlefield, it deals 4 damage to each other creature.")
        .Power(6)
        .Toughness(6)
        .Echo("{4}{R}{R}")
        .Abilities(
          TriggeredAbility(
            "When Crater Hellion enters the battlefield, it deals 4 damage to each other creature.",
            Trigger<OnZoneChanged>(t => t.To = Zone.Battlefield),
            Effect<DealDamageToCreaturesAndPlayers>(e =>
              {
                e.AmountCreature = 4;
                e.FilterCreature = (self, creature) => creature != self.Source.OwningCard;
              })
            )
        );
    }
  }
}
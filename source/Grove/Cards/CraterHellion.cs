namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class CraterHellion : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
        .TriggeredAbility(p =>
          {
            p.Text = "When Crater Hellion enters the battlefield, it deals 4 damage to each other creature.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DealDamageToCreaturesAndPlayers(amountCreature: 4,
              filterCreature: (effect, creature) => creature != effect.Source.OwningCard);
          }
        );
    }
  }
}
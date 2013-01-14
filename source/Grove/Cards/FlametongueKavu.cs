namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Targeting;
  using Core.Triggers;
  using Core.Zones;

  public class FlametongueKavu : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Flametongue Kavu")
        .ManaCost("{3}{R}")
        .Type("Creature Kavu")
        .Text("When Flametongue Kavu enters the battlefield, it deals 4 damage to target creature.")
        .FlavorText("'For dim-witted, thick-skulled genetic mutants, they have pretty good aim.{EOL}—Sisay'")
        .Power(4)
        .Toughness(2)
        .Cast(p => p.Timing = Timings.OpponentHasPermanent(
          card => card.Is().Creature && card.Life <= 4 &&
              !card.HasProtectionFrom(ManaColors.Red)))
        .Abilities(
          TriggeredAbility(
            "When Flametongue Kavu enters the battlefield, it deals 4 damage to target creature.",
            Trigger<OnZoneChanged>(t => t.To = Zone.Battlefield),
            Effect<DealDamageToTargets>(e => e.Amount = 4),
            Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()),
            TargetingAi.DealDamageSingleSelector(4)));
    }
  }
}
namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;
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
        .Timing(Timings.OpponentHasPermanent(
          card => card.Is().Creature &&
            card.Life <= 4 &&
              !card.HasProtectionFrom(ManaColors.Red)))
        .Abilities(
          TriggeredAbility(
            "When Flametongue Kavu enters the battlefield, it deals 4 damage to target creature.",
            Trigger<OnZoneChange>(t => t.To = Zone.Battlefield),
            Effect<DealDamageToTargets>(e => e.Amount = 4),
            Validator(Validators.Creature()),
            selectorAi: TargetSelectorAi.DealDamageSingleSelector(4)));
    }
  }
}
namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class FlametongueKavu : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Flametongue Kavu")
        .ManaCost("{3}{R}")
        .Type("Creature Kavu")
        .Text("When Flametongue Kavu enters the battlefield, it deals 4 damage to target creature.")
        .FlavorText("'For dim-witted, thick-skulled genetic mutants, they have pretty good aim.{EOL}—Sisay'")
        .Power(4)
        .Toughness(2)
        .Timing(Timings.OpponentControlsAPermanent(
          card => card.Is().Creature && 
          card.Life <= 4 && 
          !card.HasProtectionFrom(ManaColors.Red)))
        .Abilities(
          C.TriggeredAbility(
            "When Flametongue Kavu enters the battlefield, it deals 4 damage to target creature.",
            C.Trigger<OnZoneChange>((t, _) => t.To = Zone.Battlefield),
            C.Effect<DealDamageToTargets>(e => e.Amount = 4),
            C.Validator(Validators.Creature()),
            selectorAi: TargetSelectorAi.DealDamageSingleSelector(4)));
    }
  }
}
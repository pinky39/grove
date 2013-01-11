namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class HeatRay : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Heat Ray")
        .ManaCost("{R}")
        .Type("Instant")
        .Text("Heat Ray deals X damage to target creature.")
        .FlavorText("It's not known whether the Thran built the device to forge their wonders or to defend them.")
        .Cast(p =>
          {
            p.Timing = Timings.InstantRemovalTarget();
            p.XCalculator = VariableCost.TargetLifepointsLeft(ManaUsage.Spells);
            p.Effect = Effect<DealDamageToTargets>(e => e.Amount = Value.PlusX);
            p.EffectTargets = L(Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()));
            p.TargetingAi = TargetingAi.DealDamageSingleSelector();
          });
    }
  }
}
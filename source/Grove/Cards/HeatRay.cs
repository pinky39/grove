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
        .ManaCost("{R}").XCalculator(VariableCost.TargetLifepointsLeft(ManaUsage.Spells))
        .Type("Instant")
        .Text("Heat Ray deals X damage to target creature.")
        .FlavorText("It's not known whether the Thran built the device to forge their wonders or to defend them.")
        .Effect<DealDamageToTargets>(p => p.Amount = Value.PlusX)
        .Timing(Timings.InstantRemovalTarget())
        .Targets(
          selectorAi: TargetSelectorAi.DealDamageSingleSelector(),
          effectValidator: TargetValidator(TargetIs.Creature()));
    }
  }
}
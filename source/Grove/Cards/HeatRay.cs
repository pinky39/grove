namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;  
  
  public class HeatRay : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Heat Ray")
        .ManaCost("{R}").XCalculator(VariableCost.TargetLifepointsLeft(ManaUsage.Spells))
        .Type("Instant")
        .Text("Heat Ray deals X damage to target creature.")
        .FlavorText("It's not known whether the Thran built the device to forge their wonders or to defend them.")
        .Effect<DealDamageToTargets>(p => p.Amount = Value.PlusX)
        .Timing(Timings.TargetRemovalInstant())
        .Targets(
          aiTargetSelector: TargetSelectorAi.DealDamageSingleSelector(),
          effectValidator: C.Validator(Validators.Creature()));
    }
  }
}
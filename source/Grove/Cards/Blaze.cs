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

  public class Blaze : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Blaze")
        .ManaCost("{R}").XCalculator(VariableCost.TargetLifepointsLeft(ManaUsage.Spells))
        .Type("Sorcery")
        .Text("Blaze deals X damage to target creature or player.")
        .FlavorText("Fire never dies alone.")
        .Effect<DealDamageToTargets>(p => p.Amount = Value.PlusX)
        .Timing(Timings.MainPhases())
        .Targets(
          selectorAi: TargetSelectorAi.DealDamageSingleSelector(),
          effectValidator: Validator(Validators.CreatureOrPlayer()));
    }
  }
}
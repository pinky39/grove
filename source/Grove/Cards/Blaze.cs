namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class Blaze : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Blaze")
        .ManaCost("{R}").XCalculator(VariableCost.TargetLifepointsLeft())
        .Type("Sorcery")
        .Text("Blaze deals X damage to target creature or player.")
        .FlavorText("Fire never dies alone.")
        .Effect<DealDamageToTarget>(p => p.Amount = Value.PlusX)
        .Timing(Timings.MainPhases())
        .Targets(
          aiTargetSelector: AiTargetSelectors.DealDamage(),
          effectValidator: C.Validator(Validators.CreatureOrPlayer()));
    }
  }
}
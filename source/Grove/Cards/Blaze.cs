namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Effects;
  using Core.Modifiers;

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
        .Category(EffectCategories.DamageDealing)
        .Effect<DealDamageToTarget>((e, _) => e.Amount = Value.PlusX)
        .Target(C.Selector(
          validator: target => target.IsPlayer() || target.Is().Creature,
          scorer: Core.Ai.TargetScores.OpponentStuffScoresMore()));
    }
  }
}
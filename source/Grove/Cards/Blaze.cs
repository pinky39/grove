namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
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
        .Effect<DealDamageToTarget>((e, _) => e.Amount = Value.PlusX)
        .Timing(Timings.MainPhases())
        .Targets(
          filter: TargetFilters.DealDamage(),
          selectors: C.Selector(Selectors.CreatureOrPlayer()));
    }
  }
}
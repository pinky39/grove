namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Annul : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Annul")
        .ManaCost("{U}")
        .Type("Instant")
        .Text("Counter target artifact or enchantment spell.")
        .FlavorText("The most effective way to destroy a spell is to ensure it was never cast in the first place.")
        .Category(EffectCategories.Counterspell)
        .Timing(Timings.CounterSpell())
        .Effect<CounterTargetSpell>()
        .Targets(
          filter: TargetFilters.CounterSpell(),
          effect: C.Selector(Selectors.Counterspell("artifact", "enchantment")));
    }
  }
}
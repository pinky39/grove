namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

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
        .Target(C.Selector(target =>
          target.IsEffect() &&
            target.Effect().CanBeCountered &&
              target.Effect().Source is Card &&
                (target.Effect().Source.OwningCard.Is().Artifact || target.Effect().Source.OwningCard.Is().Enchantment)))
        .TargetFilter(TargetFilters.CounterSpell());
    }
  }
}
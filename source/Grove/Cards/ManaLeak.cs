namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class ManaLeak : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Mana Leak")
        .ManaCost("{1}{U}")
        .Type("Instant")
        .Text("Counter target spell unless its controller pays {3}.")
        .FlavorText("The fatal flaw in every plan is the assumption that you know more than your enemy.")
        .Category(EffectCategories.Counterspell)
        .Timing(Timings.CounterSpell(3))
        .Effect<CounterTargetSpell>((e, _) => e.DoNotCounterCost = 3.AsColorlessMana())
        .Targets(C.Selector(
          validator: target =>
            target.IsEffect() &&
              target.Effect().CanBeCountered &&
                target.Effect().Source is Card,
          scorer: TargetScores.OpponentStuffScoresMore()));
    }
  }
}
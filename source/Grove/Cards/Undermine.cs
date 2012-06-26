namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class Undermine : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Undermine")
        .ManaCost("{U}{U}{B}")
        .Type("Instant")
        .Text("Counter target spell. Its controller loses 3 life.")
        .FlavorText("'Which would you like first, the insult or the injury?'")
        .Category(EffectCategories.Counterspell)
        .Timing(Timings.CounterSpell())
        .Effect<CounterTargetSpell>((e, _) => e.ControllersLifeloss = 3)
        .Target(C.Selector(
          validator: target =>
            target.IsEffect() &&
              target.Effect().CanBeCountered &&
                target.Effect().Source is Card,
          scorer: TargetScores.OpponentStuffScoresMore()));
    }
  }
}
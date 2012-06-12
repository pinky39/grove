namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class Counterspell : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Counterspell")
        .ManaCost("{U}{U}")
        .Type("Instant")
        .Text("Counter target spell.")
        .FlavorText("'Your attack has been rendered harmless. It is, however, quite pretty.'{EOL}—Saprazzan vizier")
        .Category(EffectCategories.Counterspell)
        .Timing(Timings.CounterSpell())
        .Effect<CounterTargetSpell>()
        .Target(C.Selector(
          validator: target =>
            target.IsEffect() &&
              target.Effect().CanBeCountered &&
                target.Effect().Source is Card,
          scorer: Core.Ai.TargetScores.OpponentStuffScoresMore()));
    }
  }
}
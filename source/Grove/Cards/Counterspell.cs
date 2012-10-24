namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;
  
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
        .Targets(
          selectorAi: TargetSelectorAi.CounterSpell(),
          effectValidator: C.Validator(Validators.Counterspell()));
    }
  }
}
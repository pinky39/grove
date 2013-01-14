namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;
  using Core.Targeting;

  public class Counterspell : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Counterspell")
        .ManaCost("{U}{U}")
        .Type("Instant")
        .Text("Counter target spell.")
        .FlavorText("'Your attack has been rendered harmless. It is, however, quite pretty.'{EOL}—Saprazzan vizier")
        .Cast(p =>
          {
            p.Timing = Timings.CounterSpell();
            p.Category = EffectCategories.Counterspell;
            p.Effect = Effect<CounterTargetSpell>();
            p.EffectTargets = L(Target(Validators.CounterableSpell(), Zones.Stack()));
            p.TargetingAi = TargetingAi.CounterSpell();
          });
    }
  }
}
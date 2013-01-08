namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Undermine : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Undermine")
        .ManaCost("{U}{U}{B}")
        .Type("Instant")
        .Text("Counter target spell. Its controller loses 3 life.")
        .FlavorText("'Which would you like first, the insult or the injury?'")
        .Cast(p =>
          {
            p.Timing = Timings.CounterSpell();
            p.Category = EffectCategories.Counterspell;
            p.Effect = Effect<CounterTargetSpell>(e => e.ControllersLifeloss = 3);
            p.EffectTargets = L(Target(Validators.CounterableSpell(), Zones.Stack()));
            p.TargetSelectorAi = TargetSelectorAi.CounterSpell();
          });
    }
  }
}
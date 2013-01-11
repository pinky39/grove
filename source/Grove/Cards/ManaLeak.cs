namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class ManaLeak : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Mana Leak")
        .ManaCost("{1}{U}")
        .Type("Instant")
        .Text("Counter target spell unless its controller pays {3}.")
        .FlavorText("The fatal flaw in every plan is the assumption that you know more than your enemy.")
        .Cast(p =>
          {
            p.Timing = Timings.CounterSpell(3);
            p.Category = EffectCategories.Counterspell;
            p.Effect = Effect<CounterTargetSpell>(e => e.DoNotCounterCost = 3.Colorless());
            p.EffectTargets = L(Target(Validators.CounterableSpell(), Zones.Stack()));
            p.TargetingAi = TargetingAi.CounterSpell();
          });
    }
  }
}
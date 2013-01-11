namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Annul : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Annul")
        .ManaCost("{U}")
        .Type("Instant")
        .Text("Counter target artifact or enchantment spell.")
        .FlavorText("The most effective way to destroy a spell is to ensure it was never cast in the first place.")
        .Cast(p =>
          {
            p.Timing = Timings.CounterSpell();
            p.Category = EffectCategories.Counterspell;
            p.Effect = Effect<CounterTargetSpell>();
            p.EffectTargets = L(Target(
              Validators.CounterableSpell(card => card.Is().Artifact || card.Is().Enchantment),
              Zones.Stack()));
            p.TargetingAi = TargetingAi.CounterSpell();
          });
    }
  }
}
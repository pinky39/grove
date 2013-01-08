namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class PowerSink : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Power Sink")
        .ManaCost("{U}")
        .Type("Instant")
        .Text(
          "Counter target spell unless its controller pays X. If he or she doesn't, that player taps all lands with mana abilities he or she controls and empties his or her mana pool.")
        .Cast(p =>
          {
            p.Timing = Timings.CounterSpell();
            p.Category = EffectCategories.Counterspell;
            p.XCalculator = VariableCost.CounterUnlessPay();
            p.Effect = Effect<CounterTargetSpell>(e =>
              {
                e.DoNotCounterCost = e.X.GetValueOrDefault().Colorless();
                e.TapLandsEmptyPool = true;
              });
            p.EffectTargets = L(Target(Validators.CounterableSpell(), Zones.Stack()));
            p.TargetSelectorAi = TargetSelectorAi.CounterSpell();
          });
    }
  }
}
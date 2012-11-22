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
        .ManaCost("{U}").XCalculator(VariableCost.CounterUnlessPay())
        .Type("Instant")
        .Text(
          "Counter target spell unless its controller pays X. If he or she doesn't, that player taps all lands with mana abilities he or she controls and empties his or her mana pool.")
        .Category(EffectCategories.Counterspell)
        .Timing(Timings.CounterSpell())
        .Effect<CounterTargetSpell>(e =>
          {
            e.DoNotCounterCost = e.X.GetValueOrDefault().AsColorlessMana();
            e.TapLandsEmptyPool = true;
          })
        .Targets(
          TargetSelectorAi.CounterSpell(), 
          TargetValidator(TargetIs.CounterableSpell(), ZoneIs.Stack()));
    }
  }
}
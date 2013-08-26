namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.CostRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class PowerSink : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Power Sink")
        .ManaCost("{U}").HasXInCost()
        .Type("Instant")
        .Text(
          "Counter target spell unless its controller pays X. If he or she doesn't, that player taps all lands with mana abilities he or she controls and empties his or her mana pool.")
        .Cast(p =>
          {
            p.Effect = () => new CounterTargetSpell(
              doNotCounterCost: P(e => e.X.GetValueOrDefault()),
              tapLandsAndEmptyManaPool: true);

            p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());

            p.CostRule(new XIsManaAvailableToOpponentPlus1());
            p.TimingRule(new Artifical.TimingRules.WhenTopSpellIsCounterable());
            p.TargetingRule(new Artifical.TargetingRules.EffectCounterspell());
          });
    }
  }
}
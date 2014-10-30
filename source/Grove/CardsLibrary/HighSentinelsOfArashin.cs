namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.RepetitionRules;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class HighSentinelsOfArashin : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("High Sentinels of Arashin")
        .ManaCost("{3}{W}")
        .Type("Creature - Bird Soldier")
        .Text("{Flying}{EOL}High Sentinels of Arashin gets +1/+1 for each other creature you control with a +1/+1 counter on it.{EOL}{3}{W}: Put a +1/+1 counter on target creature.")
        .Power(3)
        .Toughness(4)
        .SimpleAbilities(Static.Flying)
        .StaticAbility(p =>
        {
          p.Modifier(() => new ModifyPowerToughnessForEachPermanent(
            power: 1,
            toughness: 1,
            filter: (c, m) => c.Is().Creature && c.CountersCount(CounterType.PowerToughness) > 0 && c != m.OwningCard,
            modifier: () => new IntegerIncrement()
            ));

          p.EnabledInAllZones = false;
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{3}{W}: Put a +1/+1 counter on target creature.";
          p.Cost = new PayMana("{3}{W}".Parse(), ManaUsage.Abilities, supportsRepetitions: true);

          p.Effect = () => new ApplyModifiersToTargets(() => new AddCounters(
            () => new PowerToughness(1, 1), count: 1));

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          
          p.TimingRule(new PumpTargetCardTimingRule(untilEot: false));
          p.TargetingRule(new EffectPumpInstant(1, 1, untilEot: false));
          
          p.RepetitionRule(new RepeatMaxTimes());
        });
    }
  }
}

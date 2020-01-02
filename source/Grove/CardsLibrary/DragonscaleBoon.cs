namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class DragonscaleBoon : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Dragonscale Boon")
          .ManaCost("{3}{G}")
          .Type("Instant")
          .Text("Put two +1/+1 counters on target creature and untap it.")
          .FlavorText("\"When we were lost and weary, the ainok showed us how to survive. They have earned the right to call themselves Abzan, and to wear the Scale.\"{EOL}—Anafenza, khan of the Abzan")
          .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new ApplyModifiersToTargets(() => new AddCounters(
                () => new PowerToughness(1, 1), count: 2)),
              new UntapTargetPermanents()).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TimingRule(new PumpTargetCardTimingRule(untilEot: false));
            p.TargetingRule(new EffectPumpInstant(2, 2, untilEot: false));
          });
    }
  }
}

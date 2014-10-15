namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class TitanicGrowth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Titanic Growth")
        .ManaCost("{1}{G}")
        .Type("Instant")
        .Text("Target creature gets +4/+4 until end of turn.")
        .FlavorText(
          "The massive dominate through might. The tiny survive with guile. Beware the tiny who become massive.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(4, 4) {UntilEot = true}).SetTags(EffectTag.IncreasePower,
                EffectTag.IncreaseToughness);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new PumpTargetCardTimingRule());
            p.TargetingRule(new EffectPumpInstant(4, 4));
          });
    }
  }
}
namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class Throttle : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Throttle")
        .ManaCost("{4}{B}")
        .Type("Instant")
        .Text("Target creature gets -4/-4 until end of turn.")
        .FlavorText("\"The best servants are made from those who died without a scratch.\"{EOL}—Sidisi, khan of the Sultai")
        .Cast(p =>
        {
          p.Effect = () => new ApplyModifiersToTargets(() => new AddPowerAndToughness(-4, -4) { UntilEot = true }) { ToughnessReduction = 4 };

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          p.TargetingRule(new EffectReduceToughness(4));
          p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.ReduceToughness));
        });
    }
  }
}

namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class MightOfOaks : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Might of Oaks")
        .ManaCost("{3}{G}")
        .Type("Instant")
        .Text("Target creature gets +7/+7 until end of turn.")
        .FlavorText("Suddenly, she couldn't see the acorns for the trees.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(7, 7) {UntilEot = true}).SetTags(EffectTag.IncreasePower,
                EffectTag.IncreaseToughness);
              
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectPumpInstant(7, 7));
          });
    }
  }
}
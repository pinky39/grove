namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class Humble : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Humble")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("Target creature loses all abilities and becomes 0/1 until end of turn.")
        .FlavorText("It is not your place to rule, Radiant. It may not even be mine.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new DisableAllAbilities {UntilEot = true},
              () => new SetPowerAndToughness(0, 1) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Humble, combatOnly: true));
          });
    }
  }
}
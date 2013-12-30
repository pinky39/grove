namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class Endoskeleton : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Endoskeleton")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text(
          "You may choose not to untap Endoskeleton during your untap step.{EOL}{2},{T}: Target creature gets +0/+3 for as long as Endoskeleton remains tapped.")
        .MayChooseToUntap()
        .ActivatedAbility(p =>
          {
            p.Text = "{2},{T}: Target creature gets +0/+3 for as long as Endoskeleton remains tapped.";
            p.Cost = new AggregateCost(
              new PayMana(2.Colorless(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new ApplyModifiersToTargets(() =>
              {
                var modifier = new AddPowerAndToughness(0, 3);
                modifier.AddLifetime(new ModifierSourceGetsUntapedLifetime());
                return modifier;
              });

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectPumpInstant(0, 3, untilEot: false));
          });
    }
  }
}
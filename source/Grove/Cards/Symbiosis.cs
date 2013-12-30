namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class Symbiosis : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Symbiosis")
        .ManaCost("{1}{G}")
        .Type("Instant")
        .Text("Two target creatures each get +2/+2 until end of turn.")
        .FlavorText(
          "Although the elves of Argoth always considered them a nuisance, the pixies made fine allies during the war against the machines.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(() => new AddPowerAndToughness(2, 2) {UntilEot = true})
              .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);              

            p.TargetSelector.AddEffect(trg =>
              {
                trg.Is.Creature().On.Battlefield();
                trg.MinCount = 2;
                trg.MaxCount = 2;
              });

            p.TargetingRule(new EffectPumpInstant(2, 2));
          });
    }
  }
}
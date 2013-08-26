namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class AboutFace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("About Face")
        .ManaCost("{R}")
        .Type("Instant")
        .Text("Switch target creature's power and toughness until end of turn.")
        .FlavorText("The overconfident are the most vulnerable.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new SwitchPowerAndToughness {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectSwitchPowerAndToughness());
          });
    }
  }
}
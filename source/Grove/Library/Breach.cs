namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class Breach : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Breach")
        .ManaCost("{2}{B}")
        .Type("Instant")
        .Text(
          "Target creature gets +2/+0 and gains fear until end of turn. (It can't be blocked except by artifact creatures and/or black creatures.)")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddStaticAbility(Static.Fear) {UntilEot = true},
              () => new AddPowerAndToughness(2, 0) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new OnYourTurn(Step.BeginningOfCombat));
            p.TargetingRule(new EffectBigWithoutEvasions());
          });
    }
  }
}
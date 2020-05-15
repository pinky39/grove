namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class SilkNet : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Silk Net")
        .ManaCost("{G}")
        .Type("Instant")
        .Text("Target creature gets +1/+1 and gains reach until end of turn. (It can block creatures with flying.)")
        .FlavorText("Sometimes it's possible to pull a meal out of thin air.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(1, 1) {UntilEot = true},
              () => new AddSimpleAbility(Static.Reach) {UntilEot = true})
              .SetTags(EffectTag.IncreaseToughness, EffectTag.IncreasePower, EffectTag.GainReach);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            
            p.TimingRule(new AfterOpponentDeclaresAttackers());
            p.TargetingRule(new EffectGiveReach());
          });
    }
  }
}
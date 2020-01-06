namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class JeskaiCharm : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Jeskai Charm")
        .ManaCost("{U}{R}{W}")
        .Type("Instant")
        .Text("Choose one —{EOL}• Put target creature on top of its owner's library.{EOL}• Jeskai Charm deals 4 damage to target opponent.{EOL}• Creatures you control get +1/+1 and gain lifelink until end of turn.")
        .Cast(p =>
        {
          p.Text = "{{U}}{{R}}{{W}}: Put target creature on top of its owner's library.";
          p.Effect = () => new PutTargetsOnTopOfLibrary();
          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          p.TargetingRule(new EffectPutOnTopOfLibrary());
          p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Bounce, EffectTag.CreaturesOnly));
        })
        .Cast(p =>
        {
          p.Text = "{{U}}{{R}}{{W}}: Jeskai Charm deals 4 damage to target opponent.";
          p.Effect = () => new DealDamageToTargets(4);
          p.TargetSelector.AddEffect(trg => trg.Is.Opponent());
          p.TargetingRule(new EffectDealDamage(4));
          p.TimingRule(new OnSecondMain());
        })
        .Cast(p =>
        {
          p.Text = "{{U}}{{R}}{{W}}: Creatures you control get +1/+1 and gain lifelink until end of turn.";
          p.Effect = () => new ApplyModifiersToPermanents(
            selector: (c, ctx) => c.Is().Creature && ctx.You == c.Controller,            
            modifiers: L(            
              () => new AddPowerAndToughness(1, 1){UntilEot = true},
              () => new AddStaticAbility(Static.Lifelink){UntilEot = true}
            ));

          p.TimingRule(new OnStep(Step.DeclareBlockers));
        });
    }
  }
}

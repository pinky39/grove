namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class SmiteTheMonstrous : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Smite the Monstrous")
        .ManaCost("{3}{W}")
        .Type("Instant")
        .Text("Destroy target creature with power 4 or greater.")
        .FlavorText("\"The dragons thought they were too strong to be tamed, too large to fall. And where are they now?\"{EOL}—Khibat the Revered")
        .Cast(p =>
        {
          p.Effect = () => new DestroyTargetPermanents();
          p.TargetSelector.AddEffect(trg => trg
            .Is.Card(c => c.Is().Creature && c.Power >= 4)
            .On.Battlefield());

          p.TargetingRule(new EffectDestroy());
          p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Destroy, EffectTag.CreaturesOnly));
        });
    }
  }
}

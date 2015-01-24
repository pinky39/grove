namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class Sandblast : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sandblast")
        .ManaCost("{2}{W}")
        .Type("Instant")
        .Text("Sandblast deals 5 damage to target attacking or blocking creature.")
        .FlavorText("In the Shifting Wastes, more are killed by sand than by steel.")
        .Cast(p =>
        {
          p.Effect = () => new DealDamageToTargets(5);

          p.TargetSelector.AddEffect(trg => trg.Is
            .Card(c => c.Is().Creature && (c.IsAttacker || c.IsBlocker))
            .On.Battlefield());


          p.TimingRule(new OnStep(Step.DeclareBlockers));
          p.TargetingRule(new EffectDealDamage(5));
        });
    }
  }
}

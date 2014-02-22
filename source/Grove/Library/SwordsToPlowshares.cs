namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

  public class SwordsToPlowshares : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Swords to Plowshares")
        .ManaCost("{W}")
        .Type("Instant")
        .Text("Exile target creature. Its controller gains life equal to its power.")
        .Cast(p =>
          {
            p.Effect = () => new ExileTargets(controllerGainsLifeEqualToToughness: true);
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectExileBattlefield());
            p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Exile, EffectTag.CreaturesOnly));
          });
    }
  }
}
namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;

  public class SwordsToPlowshares : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.TargetingRule(new Exile());
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}
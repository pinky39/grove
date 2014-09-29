namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class Congregate : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Congregate")
        .ManaCost("{3}{W}")
        .Type("Instant")
        .Text("Target player gains 2 life for each creature on the battlefield.")
        .FlavorText(
          "In the gathering there is strength for all who founder, renewal for all who languish, love for all who sing.")
        .Cast(p =>
          {
            p.Effect = () => new TargetPlayerGainsLifeEqualToCreatureCount(multiplier: 2);
            p.TargetSelector.AddEffect(trg => trg.Is.Player());
            p.TimingRule(new OnEndOfOpponentsTurn());
            p.TimingRule(new WhenPermanentCountIs(3, c => c.Is().Creature));
            p.TargetingRule(new EffectYou());
          });
    }
  }
}
namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class Opportunity : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Opportunity")
        .ManaCost("{4}{U}{U}")
        .Type("Instant")
        .Text("Target player draws four cards.")
        .FlavorText(
          "He cocooned himself alone in his workshop for months. When he finally emerged, all broad grins and excited chatter, I knew he'd found his answer.")
        .Cast(p =>
          {
            p.Effect = () => new TargetPlayerDrawsCards(4);
            p.TargetSelector.AddEffect(trg => trg.Is.Player());

            p.TimingRule(new OnEndOfOpponentsTurn());
            p.TargetingRule(new EffectYou());
          });
    }
  }
}
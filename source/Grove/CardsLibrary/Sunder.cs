namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;

  public class Sunder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sunder")
        .ManaCost("{3}{U}{U}")
        .Type("Instant")
        .Text("Return all lands to their owners' hands.")
        .FlavorText(
          "The flow of time was disrupted; like a flooding river it rose from its banks. Tolaria was drowned in an instant that stretched toward infinity.")
        .OverrideScore(new ScoreOverride {Hand = 50})
        .Cast(p =>
          {
            p.Effect = () => new ReturnAllPermanentsToHand(c => c.Is().Land);
            p.TimingRule(new OnEndOfOpponentsTurn());
          });
    }
  }
}
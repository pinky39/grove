namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;

  public class Windfall : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Windfall")
        .ManaCost("{2}{U}")
        .Type("Sorcery")
        .Text(
          "Each player discards his or her hand, then draws cards equal to the greatest number of cards a player discarded this way.")
        .FlavorText("To fill your mind with knowledge, we must start by emptying it.")
        .Cast(p =>
          {
            p.Effect = () => new EachPlayerDiscardsHandAndDrawsGreatestDiscardedCount();
            p.TimingRule(new OnSecondMain());
            p.TimingRule(new WhenYouHaveBiggerHand(-2));
          });
    }
  }
}
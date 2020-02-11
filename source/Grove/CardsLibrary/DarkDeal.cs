namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class DarkDeal : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dark Deal")
        .ManaCost("{2}{B}")
        .Type("Sorcery")
        .Text("Each player discards all the cards in his or her hand, then draws that many cards minus one.")
        .FlavorText("The first khans of the Sultai relied on the magic of the rakshasa to ensure the survival of the clan.")
        .Cast(p =>
        {
          p.Effect = () => new EachPlayerDiscardsHandAndDrawsThatManyCardsMinusOne();
          p.TimingRule(new OnFirstMain());
          p.TimingRule(new WhenYourHandCountIs(minCount: 3, selector: c => c.Type.Land));
          p.TimingRule(new WhenOpponentsHandCountIs(maxCount: 1));
        });
    }
  }
}

namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class BloodfireMentor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bloodfire Mentor")
        .ManaCost("{2}{R}")
        .Type("Creature — Efreet Shaman")
        .Text("{2}{U}, {T}: Draw a card, then discard a card.")
        .FlavorText("The adept underwent months of preparation to withstand pain, until he was finally ready to receive the efreet master's teachings.")
        .Power(0)
        .Toughness(5)
        .ActivatedAbility(p =>
        {
          p.Text = "{2}{U}, {T}: Draw a card, then discard a card.";
          p.Cost = new AggregateCost(
            new PayMana("{2}{U}".Parse(), ManaUsage.Abilities),
            new Tap());

          p.Effect = () => new DrawCards(1, discardCount: 1);
          p.TimingRule(new OnEndOfOpponentsTurn());
        });
    }
  }
}

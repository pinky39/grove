namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class JasmineSeer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Jasmine Seer")
        .ManaCost("{3}{W}")
        .Type("Creature Human Wizard")
        .Text(
          "{2}{W},{T}: Reveal any number of white cards in your hand. You gain 2 life for each card revealed this way.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{2}{W},{T}: Reveal any number of white cards in your hand. You gain 2 life for each card revealed this way.";
            p.Cost = new AggregateCost(
              new PayMana("{2}{W}".Parse(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new GainLifeForEachRevealedCard(c => c.HasColor(CardColor.White), 2);
            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.HasColor(CardColor.White)));
            p.TimingRule(new Any(new OnEndOfOpponentsTurn(), new WhenOwningCardWillBeDestroyed()));
          });
    }
  }
}
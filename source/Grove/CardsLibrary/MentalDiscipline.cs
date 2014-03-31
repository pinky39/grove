namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class MentalDiscipline : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mental Discipline")
        .ManaCost("{1}{U}{U}")
        .Type("Enchantment")
        .Text("{1}{U}, Discard a card: Draw a card.")
        .FlavorText("Barrin drowned his doubts about Urza's project by delving ever deeper into its details.")
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{U}, Discard a card: Draw a card.";
            p.Cost = new AggregateCost(
              new PayMana("{1}{U}".Parse(), ManaUsage.Abilities),
              new DiscardTarget());

            p.Effect = () => new DrawCards(1);

            p.TargetSelector.AddCost(trg => trg.Is.Card().In.OwnersHand());
            p.TimingRule(new DefaultCyclingTimingRule());
            p.TargetingRule(new CostDiscardCard());
          });
    }
  }
}
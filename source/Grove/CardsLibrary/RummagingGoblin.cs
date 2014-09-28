namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;

    public class RummagingGoblin : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Rummaging Goblin")
                .ManaCost("{2}{R}")
                .Type("Creature — Goblin Rogue")
                .Text("{T}, Discard a card: Draw a card.")
                .FlavorText("To a goblin, value is based on the four S's: shiny, stabby, smelly, and super smelly.")
                .Power(1)
                .Toughness(1)
                .ActivatedAbility(p =>
                {
                    p.Text = "{T}, Discard a card: Draw a card.";
                    p.Cost = new AggregateCost(
                      new TapOwner(),
                      new DiscardTarget());

                    p.Effect = () => new DrawCards(1);

                    p.TargetSelector.AddCost(trg => trg.Is.Card().In.OwnersHand());
                    p.TimingRule(new DefaultCyclingTimingRule());
                    p.TargetingRule(new CostDiscardCard());
                });
        }
    }
}

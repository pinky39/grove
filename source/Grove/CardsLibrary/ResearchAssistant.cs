namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using Costs;
    using Effects;

    public class ResearchAssistant : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
              .Named("Research Assistant")
              .ManaCost("{1}{U}")
              .Type("Creature - Human Wizard")
              .Text("{3}{U},{T}: Draw a card, then discard a card.")
              .FlavorText("There are many words and phrases that can cause an experienced wizard to tremble in fear. Chief among them is \"oops.\"")
              .Power(1)
              .Toughness(3)
              .ActivatedAbility(p =>
              {
                  p.Text = "{3}{U},{T}: Draw a card, then discard a card.";
                  p.Cost = new AggregateCost(
                      new PayMana("{3}{U}".Parse(), ManaUsage.Abilities),
                      new Tap());                      

                  p.Effect = () => new DrawCards(1, discardCount: 1);
              });
        }
    }
}

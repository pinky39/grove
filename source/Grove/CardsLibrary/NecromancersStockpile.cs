namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using Costs;
  using Effects;

  public class NecromancersStockpile : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Necromancer's Stockpile")
        .ManaCost("{1}{B}")
        .Type("Enchantment")
        .Text("{1}{B}, Discard a creature card: Draw a card. If the discarded card was a Zombie card, put a 2/2 black Zombie creature token onto the battlefield tapped.")
        .FlavorText("The experiments decided to perform some research of their own.")
        .ActivatedAbility(p =>
        {
          p.Text = "{1}{B}, Discard a creature card: Draw a card. If the discarded card was a Zombie card, put a 2/2 black Zombie creature token onto the battlefield tapped.";

          p.Cost = new AggregateCost(
            new PayMana("{1}{B}".Parse(), ManaUsage.Abilities),
            new DiscardTarget());

          p.Effect = () => new DrawCardsCreateTokens(count: 1, effect: new CreateTokens(
            count: 1,
            token: Card
              .Named("Zombie")
              .Power(2)
              .Toughness(2)
              .Type("Token Creature - Zombie")
              .Colors(CardColor.Black),
            afterTokenComesToPlay: (token, game) => token.Tap()),
            createTokenIf: P(e => e.Targets.Cost.First().Card().Is("Zombie"), EvaluateAt.AfterCost));

          p.TargetSelector.AddCost(trg => trg.Is.Creature().In.OwnersHand());
          p.TargetingRule(new CostDiscardCard());
        });
    }
  }
}

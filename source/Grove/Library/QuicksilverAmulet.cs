namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class QuicksilverAmulet : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Quicksilver Amulet")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text("{4},{T}: You may put a creature card from your hand onto the battlefield.")
        .FlavorText("Wonderful You got a lion on your first try. Now put it back.")
        .ActivatedAbility(p =>
          {
            p.Text = "{4},{T}: You may put a creature card from your hand onto the battlefield.";

            p.Cost = new AggregateCost(
              new PayMana(4.Colorless(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new PutSelectedCardToBattlefield(
              text: "Select a creature in your hand.",
              zone: Zone.Hand,
              validator: card => card.Is().Creature);

            p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new OnEndOfOpponentsTurn()));
            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.Is().Creature));
          });
    }
  }
}
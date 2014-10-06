namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TimingRules;

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

            p.Effect = () => new PutSelectedCardsToBattlefield(
              text: "Select a creature in your hand.",
              zone: Zone.Hand,
              validator: card => card.Is().Creature);

            p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new OnEndOfOpponentsTurn()));
            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.Is().Creature));
          });
    }
  }
}
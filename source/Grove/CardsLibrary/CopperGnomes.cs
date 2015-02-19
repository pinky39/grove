namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class CopperGnomes : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Copper Gnomes")
        .ManaCost("{2}")
        .Type("Artifact Creature Gnome")
        .Text("{4}, Sacrifice Copper Gnomes: You may put an artifact card from your hand onto the battlefield.")
        .FlavorText(
          "Start with eleven gnomes and a room of parts, and come morning you'll have ten and a monster the likes of which you've never seen.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{4}, Sacrifice Copper Gnomes: You may put an artifact card from your hand onto the battlefield.";
            p.Cost = new AggregateCost(
              new PayMana(4.Colorless()),
              new Sacrifice());
            p.Effect = () => new PutSelectedCardsToBattlefield(
              text: "Select an artifact in your hand.",
              fromZone: Zone.Hand,
              validator: card => card.Is().Artifact
              );
            p.TimingRule(new Any(new WhenOwningCardWillBeDestroyed(), new OnEndOfOpponentsTurn()));
            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.Is().Artifact));
          }
        );
    }
  }
}
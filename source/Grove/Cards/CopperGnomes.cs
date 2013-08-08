namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Zones;

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
              new PayMana(4.Colorless(), ManaUsage.Abilities),
              new Sacrifice());
            p.Effect = () => new PutSelectedCardToBattlefield(
              text: "Select an artifact in your hand.",
              zone: Zone.Hand,
              validator: card => card.Is().Artifact
              );
            p.TimingRule(new Any(new OwningCardWillBeDestroyed(), new EndOfTurn()));
            p.TimingRule(new ControllerHandCountIs(minCount: 1, selector: c => c.Is().Artifact));
          }
        );
    }
  }
}
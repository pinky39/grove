namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Zones;

  public class ShowAndTell : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Show and Tell")
        .ManaCost("{2}{U}")
        .Type("Sorcery")
        .Text(
          "Each player may put an artifact, creature, enchantment, or land card from his or her hand onto the battlefield.")
        .FlavorText("At the academy, 'show and tell' too often becomes 'run and hide.'")
        .Cast(p =>
          {
            p.Effect =
              () =>
                new EachPlayerReturnsCardFromZoneToBattlefield(Zone.Hand,
                  c => c.Is().Creature || c.Is().Artifact || c.Is().Enchantment || c.Is().Land);
            p.TimingRule(new ControllerHandCountIs(1,
              selector: c => c.ConvertedCost >= 6 && (c.Is().Creature || c.Is().Artifact || c.Is().Enchantment)));
          });
    }
  }
}
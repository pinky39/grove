namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;

  public class ShowAndTell : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.Effect = () => new EachPlayerPutsCardToBattlefield(
              zone: Zone.Hand,
              filter: c => c.Is().Creature || c.Is().Artifact || c.Is().Enchantment || c.Is().Land);              
            
            p.TimingRule(new WhenYourHandCountIs(1,
              selector: c => c.ConvertedCost >= 6 && (c.Is().Creature || c.Is().Artifact || c.Is().Enchantment)));
          });
    }
  }
}
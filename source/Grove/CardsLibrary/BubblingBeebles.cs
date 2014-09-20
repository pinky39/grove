namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class BubblingBeebles : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bubbling Beebles")
        .ManaCost("{4}{U}")
        .Type("Creature Beeble")
        .Text("Bubbling Beebles can't be blocked as long as defending player controls an enchantment.")
        .FlavorText(
          "Chancellor Rayne canceled the annual beeble roast. I should have married a crueler woman.")
        .Power(3)
        .Toughness(3)
        .SimpleAbilities(Static.UnblockableIfDedenderHasEnchantments);
    }
  }
}
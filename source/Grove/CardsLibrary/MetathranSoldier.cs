namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class MetathranSoldier : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Metathran Soldier")
        .ManaCost("{1}{U}")
        .Type("Creature Metathran Soldier")
        .Text("Metathran Soldier can't be blocked.")
        .FlavorText(
          "Just as Serra crafted angels of light and faith, Urza constructed an army of sorcery and power to resist the coming invasion.")
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.Unblockable);
    }
  }
}
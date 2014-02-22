namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

  public class LoneWolf : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Lone Wolf")
        .ManaCost("{2}{G}")
        .Type("Creature Wolf")
        .Text("You may have Lone Wolf assign its combat damage as though it weren't blocked.")
        .FlavorText("A wolf without a pack is either a survivor or a brute.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.AssignsDamageAsThoughItWasntBlocked);
    }
  }
}
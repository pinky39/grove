namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class TauntingElf : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Taunting Elf")
        .ManaCost("{G}")
        .Type("Creature Elf")
        .Text("All creatures able to block Taunting Elf do so.")
        .FlavorText(
          "Much to Multani's chagrin, Rofellos gleefully tutored Yavimaya's elves on the rudest and most vulgar words spoken in Llanowar.")
        .Power(0)
        .Toughness(1)
        .SimpleAbilities(Static.Lure);
    }
  }
}
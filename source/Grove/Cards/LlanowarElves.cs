namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.CardDsl;

  public class LlanowarElves : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
      .Named("Llanowar Elves")
      .ManaCost("{G}")
      .Type("Creature - Elf Druid")
      .Text("{T}: Add {G} to your mana pool.")
      .FlavorText("One bone broken for every twig snapped underfoot.{EOL}—Llanowar penalty for trespassing")
      .Power(1)
      .Toughness(1)
      .Abilities(
        C.ManaAbility(Mana.Green, "{T}: Add {G} to your mana pool."));
    }
  }  
}
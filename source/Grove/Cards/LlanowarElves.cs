namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Mana;

  public class LlanowarElves : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Llanowar Elves")
        .ManaCost("{G}")
        .Type("Creature - Elf Druid")
        .Text("{T}: Add {G} to your mana pool.")
        .FlavorText("One bone broken for every twig snapped underfoot.")
        .Power(1)
        .Toughness(1)
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} to your mana pool.";
            p.ManaAmount(ManaUnit.Green);
          });        
    }
  }
}
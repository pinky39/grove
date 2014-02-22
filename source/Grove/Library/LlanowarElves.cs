namespace Grove.Library
{
  using System.Collections.Generic;
  using Grove.Gameplay;

  public class LlanowarElves : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.ManaAmount(Mana.Green);
          });
    }
  }
}
namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;

  public class PriestOfTitania : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Priest of Titania")
        .ManaCost("{1}{G}")
        .Type("Creature Elf Druid")
        .Text("{T}: Add {G} to your mana pool for each Elf on the battlefield.")
        .FlavorText("Titania rewards all who honor the forest by making them a living part of it.")
        .Power(1)
        .Toughness(1)
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} to your mana pool for each Elf on the battlefield.";
            p.ManaAmount(ManaColor.Green, c => c.Is("elf"), ControlledBy.Any);
          });
    }
  }
}
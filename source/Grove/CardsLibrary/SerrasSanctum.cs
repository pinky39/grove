namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class SerrasSanctum : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Serra's Sanctum")
        .Type("Legendary Land")
        .Text("{T}: Add {W} to your mana pool for each enchantment you control.")
        .FlavorText("A fragile cocoon of dreaming will.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {W} to your mana pool for each enchantment you control.";
            p.ManaAmount(ManaColor.White, c => c.Is().Enchantment);
          }
        );
    }
  }
}
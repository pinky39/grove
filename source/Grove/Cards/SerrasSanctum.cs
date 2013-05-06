namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;

  public class SerrasSanctum : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
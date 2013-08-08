namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class BullHippo : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bull Hippo")
        .ManaCost("{3}{G}")
        .Type("Creature Hippo")
        .Text("{Islandwalk} (This creature is unblockable as long as defending player controls an Island.)")
        .FlavorText("How could you not hear it approach? It's a hippo!")
        .Power(3)
        .Toughness(3)
        .SimpleAbilities(Static.Islandwalk);
    }
  }
}
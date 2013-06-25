namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class TreetopRangers : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Treetop Rangers")
        .ManaCost("{2}{G}")
        .Type("Creature Elf")
        .Text("Treetop Rangers can't be blocked except by creatures with flying.")
        .FlavorText("If you can't catch them, cut the trees down from beneath them. Force them to fight on our terms.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.CanOnlyBeBlockedByCreaturesWithFlying);
    }
  }
}
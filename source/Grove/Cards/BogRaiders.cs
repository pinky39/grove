namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;

  public class BogRaiders : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Bog Raiders")
        .ManaCost("{2}{B}")
        .Type("Creature Zombie")
        .Text("{Swampwalk} (This creature is unblockable as long as defending player controls a Swamp.)")
        .FlavorText(
          "Let weak feed on weak, that we may divine the nature of strength.")
        .Power(2)
        .Toughness(2)
        .StaticAbilities(Static.Swampwalk);
    }
  }
}
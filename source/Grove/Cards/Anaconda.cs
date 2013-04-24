namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;

  public class Anaconda : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Anaconda")
        .ManaCost("{3}{G}")
        .Type("Creature Snake")
        .Text("{Swampwalk} (This creature is unblockable as long as defending player controls a Swamp.)")
        .FlavorText(
          "If you're smaller than the anaconda, it considers you food. If you're larger than the anaconda, it considers you a lot of food.")
        .Power(3)
        .Toughness(3)
        .StaticAbilities(Static.Swampwalk);
    }
  }
}
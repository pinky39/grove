namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Factory;

  public class LeatherbackBaloth : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Leatherback Baloth")
        .ManaCost("{G}{G}{G}")
        .Type("Creature - Beast")
        .FlavorText(
          "Heavy enough to withstand the Roil, leatherback skeletons are havens for travelers in storms and landshifts.")
        .Power(4)
        .Toughness(5);
    }
  }
}
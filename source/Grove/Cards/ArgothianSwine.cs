namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;

  public class ArgothianSwine : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Argothian Swine")
        .ManaCost("{3}{G}")
        .Type("Creature Boar")
        .Text("{Trample}")
        .FlavorText("In Argoth, the shortest route between two points is the one the swine make.")
        .Power(3)
        .Toughness(3)
        .StaticAbilities(Static.Trample);
    }
  }
}
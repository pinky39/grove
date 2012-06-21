namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.CardDsl;

  public class ArgothianSwine : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Argothian Swine")
        .ManaCost("{3}{G}")
        .Type("Creature Boar")
        .Text("{Trample}")
        .FlavorText("In Argoth, the shortest route between two points is the one the swine make.")
        .Power(3)
        .Toughness(3)
        .Abilities(
          StaticAbility.Trample);
    }
  }
}
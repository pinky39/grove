namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Dsl;

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
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Trample);
    }
  }
}
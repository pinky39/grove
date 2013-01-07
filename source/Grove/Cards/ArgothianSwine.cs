namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Dsl;

  public class ArgothianSwine : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Argothian Swine")
        .ManaCost("{3}{G}")
        .Type("Creature Boar")
        .Text("{Trample}")
        .FlavorText("In Argoth, the shortest route between two points is the one the swine make.")
        .Power(3)
        .Toughness(3)        
        .Abilities(
          Static.Trample);
    }
  }
}
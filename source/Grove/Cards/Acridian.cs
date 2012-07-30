namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;

  public class Acridian : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Acridian")
        .ManaCost("{1}{G}")
        .Type("Creature Insect")
        .Text(
          "{Echo} {1}{G}")
        .FlavorText(
          "The elves of Argoth were trained to ride these creatures, even when their mounts traveled upside-down.")
        .Power(2)
        .Toughness(4)
        .Timing(Timings.Creatures())
        .Echo("{1}{G}");
    }
  }
}
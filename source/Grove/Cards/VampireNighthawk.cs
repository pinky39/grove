namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;

  public class VampireNighthawk : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Vampire Nighthawk")
        .ManaCost("{1}{B}{B}")
        .Type("Creature - Vampire Shaman")
        .Text("{Flying}{EOL}{Deathtouch},{Lifelink}")
        .Power(2)
        .Toughness(3)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Deathtouch,
          Static.Lifelink,
          Static.Flying
        );
    }
  }
}
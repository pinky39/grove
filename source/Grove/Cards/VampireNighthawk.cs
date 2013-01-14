namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;

  public class VampireNighthawk : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Vampire Nighthawk")
        .ManaCost("{1}{B}{B}")
        .Type("Creature - Vampire Shaman")
        .Text("{Flying}{EOL}{Deathtouch},{Lifelink}")
        .Power(2)
        .Toughness(3)
        .Abilities(
          Static.Deathtouch,
          Static.Lifelink,
          Static.Flying
        );
    }
  }
}
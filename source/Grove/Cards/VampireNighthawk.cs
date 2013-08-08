namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class VampireNighthawk : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Vampire Nighthawk")
        .ManaCost("{1}{B}{B}")
        .Type("Creature - Vampire Shaman")
        .Text("{Flying}{EOL}{Deathtouch},{Lifelink}")
        .Power(2)
        .Toughness(3)
        .SimpleAbilities(
          Static.Deathtouch,
          Static.Lifelink,
          Static.Flying
        );
    }
  }
}
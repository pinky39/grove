namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class TyphoidRats : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Typhoid Rats")
        .ManaCost("{B}")
        .Type("Creature — Rat")
        .Text("{Deathtouch}{I}(Any amount of damage this deals to a creature is enough to destroy it.){/I}")
        .FlavorText(
          "Kidnappers caught in Havengul are given two choices: languish in prison or become rat catchers. The smart ones go to prison.")
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.Deathtouch);
    }
  }
}
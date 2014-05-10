namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class ThornElemental : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thorn Elemental")
        .ManaCost("{5}{G}{G}")
        .Type("Creature Elemental")
        .Text("You may have Thorn Elemental assign its combat damage as though it weren't blocked.")
        .FlavorText("Rain from this storm leaves you pinned to the ground like an insect.")
        .Power(7)
        .Toughness(7)
        .SimpleAbilities(Static.AssignsDamageAsThoughItWasntBlocked);
    }
  }
}
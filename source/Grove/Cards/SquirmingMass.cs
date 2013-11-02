namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class SquirmingMass : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Squirming Mass")
        .ManaCost("{1}{B}")
        .Type("Creature Horror")
        .Text("{Fear} (This creature can't be blocked except by artifact creatures and/or black creatures.)")
        .FlavorText("Only the coldest hearts and the strongest stomachs can stand against it.")
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.Fear);
    }
  }
}
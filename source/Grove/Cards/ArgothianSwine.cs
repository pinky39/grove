namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class ArgothianSwine : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Argothian Swine")
        .ManaCost("{3}{G}")
        .Type("Creature Boar")
        .Text("{Trample}")
        .FlavorText("In Argoth, the shortest route between two points is the one the swine make.")
        .Power(3)
        .Toughness(3)
        .SimpleAbilities(Static.Trample);
    }
  }
}
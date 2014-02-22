namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

  public class Anaconda : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Anaconda")
        .ManaCost("{3}{G}")
        .Type("Creature Snake")
        .Text("{Swampwalk} (This creature is unblockable as long as defending player controls a Swamp.)")
        .FlavorText(
          "If you're smaller than the anaconda, it considers you food. If you're larger than the anaconda, it considers you a lot of food.")
        .Power(3)
        .Toughness(3)
        .SimpleAbilities(Static.Swampwalk);
    }
  }
}
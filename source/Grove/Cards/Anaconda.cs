namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.CardDsl;

  public class Anaconda : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Anaconda")
        .ManaCost("{3}{G}")
        .Type("Creature Snake")
        .Text("{Swampwalk} (This creature is unblockable as long as defending player controls a Swamp.)")
        .FlavorText(
          "If you're smaller than the anaconda, it considers you food. If you're larger than the anaconda, it considers you a lot of food.")
        .Power(3)
        .Toughness(3)
        .Abilities(
          StaticAbility.Swampwalk
        );
    }
  }
}
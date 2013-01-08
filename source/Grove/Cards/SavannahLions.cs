namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;

  public class SavannahLions : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Savannah Lions")
        .ManaCost("{W}")
        .Type("Creature Cat")
        .FlavorText(
          "The traditional kings of the jungle command a healthy respect in other climates as well. Relying mainly on their stealth and speed, Savannah Lions can take a victim by surprise, even in the endless flat plains of their homeland.")
        .Power(2)
        .Toughness(1);
    }
  }
}
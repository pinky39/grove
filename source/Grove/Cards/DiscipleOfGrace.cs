namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;

  public class DiscipleOfGrace : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Disciple of Grace")
        .ManaCost("{1}{W}")
        .Type("Creature - Human Cleric")
        .Text("Protection from black{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("Beauty is beyond law.")
        .Power(1)
        .Toughness(2)
        .Protections(CardColor.Black)
        .Cycling("{2}");
    }
  }
}
namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Mana;
  using Core.Dsl;

  public class DiscipleOfGrace : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Disciple of Grace")
        .ManaCost("{1}{W}")
        .Type("Creature - Human Cleric")
        .Text("Protection from black{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("Beauty is beyond law.")
        .Power(1)
        .Toughness(2)
        .Timing(Timings.Creatures())
        .Protections(ManaColors.Black)
        .Cycling("{2}");
    }
  }
}
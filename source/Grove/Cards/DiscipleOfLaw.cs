namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Mana;

  public class DiscipleOfLaw : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Disciple of Law")
        .ManaCost("{1}{W}")
        .Type("Creature - Human Cleric")
        .Text("Protection from red{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("A religious order for religious order.")
        .Power(1)
        .Toughness(2)
        .Timing(Timings.Creatures())
        .Protections(ManaColors.Red)
        .Cycling("{2}");
    }
  }
}
namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Mana;
  using Core.Dsl;

  public class WhiteKnight : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("White Knight")
        .ManaCost("{W}{W}")
        .Type("Creature Human Knight")
        .Text("{First strike}, protection from black")
        .FlavorText(
          "Out of the blackness and stench of the engulfing swamp emerged a shimmering figure. Only the splattered armor and ichor-stained sword hinted at the unfathomable evil the knight had just laid waste.")
        .Power(2)
        .Toughness(2)
        .Protections(ManaColors.Black)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.FirstStrike
        );
    }
  }
}
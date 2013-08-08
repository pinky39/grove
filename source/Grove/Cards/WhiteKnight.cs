namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Characteristics;
  using Gameplay.Misc;

  public class WhiteKnight : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("White Knight")
        .ManaCost("{W}{W}")
        .Type("Creature Human Knight")
        .Text("{First strike}, protection from black")
        .FlavorText(
          "Out of the blackness and stench of the engulfing swamp emerged a shimmering figure. Only the splattered armor and ichor-stained sword hinted at the unfathomable evil the knight had just laid waste.")
        .Power(2)
        .Toughness(2)
        .Protections(CardColor.Black)
        .SimpleAbilities(Static.FirstStrike);
    }
  }
}
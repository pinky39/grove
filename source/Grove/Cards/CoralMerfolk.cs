namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Misc;

  public class CoralMerfolk : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Coral Merfolk")
        .ManaCost("{1}{U}")
        .Type("Creature Merfolk")
        .FlavorText(
          "It is not unusual for a single family of coral merfolk to spend centuries carefully guiding the growth of the reefs where they make their home.")
        .Power(2)
        .Toughness(1);
    }
  }
}
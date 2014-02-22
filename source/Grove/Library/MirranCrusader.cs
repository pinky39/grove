namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

  public class MirranCrusader : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mirran Crusader")
        .ManaCost("{1}{W}{W}")
        .Type("Creature Human Knight")
        .Text("Double strike, protection from black and from green")
        .FlavorText("A symbol of what Mirrodin once was and hope for what it will be again.")
        .Power(2)
        .Toughness(2)
        .Protections(CardColor.Black)
        .Protections(CardColor.Green)
        .SimpleAbilities(Static.DoubleStrike);
    }
  }
}
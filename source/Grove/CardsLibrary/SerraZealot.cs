namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class SerraZealot : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Serra Zealot")
        .ManaCost("{W}")
        .Type("Creature Human Soldier")
        .Text("{First strike}")
        .FlavorText(
          "The humans are useful in their way, but they must be commanded as the builder commands the stone. Be soft with them, and they will become soft.")
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.FirstStrike);
    }
  }
}
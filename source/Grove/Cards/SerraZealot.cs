namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;

  public class SerraZealot : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Serra Zealot")
        .ManaCost("{W}")
        .Type("Creature Human Soldier")
        .Text("{First strike}")
        .FlavorText(
          "he humans are useful in their way, but they must be commanded as the builder commands the stone. Be soft with them, and they will become soft.")
        .Power(1)
        .Toughness(1)
        .StaticAbilities(Static.FirstStrike);
    }
  }
}
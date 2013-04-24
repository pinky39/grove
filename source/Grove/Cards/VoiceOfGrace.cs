namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;

  public class VoiceOfGrace : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Voice of Grace")
        .ManaCost("{3}{W}")
        .Type("Creature Angel")
        .Text("{Flying}, protection from black")
        .FlavorText(
          "Opposite Law is Grace, and Grace must be preserved. If the strands of Grace are unraveled, its design will be lost, and the people with it.")
        .Power(2)
        .Toughness(2)
        .Protections(CardColor.Black)
        .StaticAbilities(Static.Flying);
    }
  }
}
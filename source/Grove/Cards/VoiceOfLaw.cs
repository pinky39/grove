namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Characteristics;
  using Gameplay.Misc;

  public class VoiceOfLaw : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Voice of Law")
        .ManaCost("{3}{W}")
        .Type("Creature Angel")
        .Text("{Flying}, protection from red")
        .FlavorText(
          "Life's balance is as a star: on one point is Law, and Law must be upheld. If the knots of order are loosened, chaos will spill through.")
        .Power(2)
        .Toughness(2)
        .Protections(CardColor.Red)
        .SimpleAbilities(Static.Flying);
    }
  }
}
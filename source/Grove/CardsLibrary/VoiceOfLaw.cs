namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class VoiceOfLaw : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
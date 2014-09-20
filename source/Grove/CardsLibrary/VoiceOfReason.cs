namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class VoiceOfReason : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Voice of Reason")
        .ManaCost("{3}{W}")
        .Type("Creature Angel")
        .Text("{Flying}, protection from blue")
        .FlavorText(
          "Next to Grace is Reason, and Reason must be retained. If the web of Reason comes unwoven, madness will escape.")
        .Power(2)
        .Toughness(2)
        .Protections(CardColor.Blue)
        .SimpleAbilities(Static.Flying);
    }
  }
}
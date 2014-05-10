namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class VoiceOfDuty : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Voice of Duty")
        .ManaCost("{3}{W}")
        .Type("Creature Angel")
        .Text("{Flying}, protection from green")
        .FlavorText(
          "Next to Law is Duty, and Duty must be obeyed. If the frame of Duty is broken, none shall weave life's fabric.")
        .Power(2)
        .Toughness(2)
        .Protections(CardColor.Green)
        .SimpleAbilities(Static.Flying);
    }
  }
}
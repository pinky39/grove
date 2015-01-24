namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class AvenSkirmisher : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Aven Skirmisher")
        .ManaCost("{W}")
        .Type("Creature - Bird Warrior")
        .Text("{Flying}")
        .FlavorText("\"We do not hide from the dragons that pretend to rule the skies. If we did, the dragons would become our rulers, and our way of life would be lost.\"")
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.Flying);
    }
  }
}

namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class HulkingOgre : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hulking Ogre")
        .ManaCost("{2}{R}")
        .Type("Creature Ogre")
        .Text("Hulking Ogre can't block.")
        .FlavorText("The Keldons' ogre campaign provided more body parts than my meager facility could properly use.")
        .Power(3)
        .Toughness(3)
        .SimpleAbilities(Static.CannotBlock);
    }
  }
}
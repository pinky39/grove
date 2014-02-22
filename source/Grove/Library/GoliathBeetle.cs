namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

  public class GoliathBeetle : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Goliath Beetle")
        .ManaCost("{2}{G}")
        .Type("Creature Insect")
        .Text("{Trample}")
        .FlavorText("The balance of nature dictates that one day humans will be stepped on by bugs.")
        .Power(3)
        .Toughness(1)
        .SimpleAbilities(Static.Trample);        
    }
  }
}
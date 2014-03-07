namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
    
  public class CapashenTemplar : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Capashen Templar")
        .ManaCost("{2}{W}")
        .Type("Creature Human Knight")
        .Text("{W}: Capashen Templar gets +0/+1 until end of turn.")
        .FlavorText("Their shields are Benalia's outermost battlements.")
        .Power(2)
        .Toughness(2)        
        .Pump(
          cost: "{W}".Parse(),
          text: "{W}: Capashen Templar gets +0/+1 until end of turn.",
          powerIncrease: 0,
          toughnessIncrease: 1);
    }
  }
}
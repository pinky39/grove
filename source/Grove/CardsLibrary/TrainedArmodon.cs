namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class TrainedArmodon : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Trained Armodon")
        .ManaCost("{1}{G}{G}")
        .Type("Creature - Elephant")
        .FlavorText("Armodons are trained to step on things - enemy things.")
        .Power(3)
        .Toughness(3);
    }
  }
}
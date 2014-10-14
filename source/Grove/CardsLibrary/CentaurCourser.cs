namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class CentaurCourser : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Centaur Courser")
        .ManaCost("{2}{G}")
        .Type("Creature - Centaur Warrior")
        .FlavorText("\"The centaurs are truly free. Never will they be tamed by temptation or controlled by fear. They live in total harmony, a feat not yet achieved by our kind.\"{EOL}—Ramal, sage of Westgate")
        .Power(3)
        .Toughness(3);
    }
  }
}

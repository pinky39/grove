namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class SummitProwler : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Summit Prowler")
        .ManaCost("{2}{R}{R}")
        .Type("Creature - Yeti")
        .FlavorText("\"Do you hunt the yetis of the high peaks, stripling? They are as fierce as the bear that fears no foe and as sly as the mink that creeps unseen. You will be as much prey as they.\"{EOL}—Nitula, the Hunt Caller")
        .Power(4)
        .Toughness(3);
    }
  }
}

namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class OreskosSwiftclaw : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Oreskos Swiftclaw")
        .ManaCost("{1}{W}")
        .Type("Creature — Cat Warrior")
        .FlavorText(
          "After the Battle of Pharagax Bridge, the Champion spent many months among the leonin of Oreskos. She found that they were quick to take offense, not because they were thin-skinned, but because they were always eager for a fight.{EOL}—The Theriad")
        .Power(3)
        .Toughness(1);
    }
  }
}
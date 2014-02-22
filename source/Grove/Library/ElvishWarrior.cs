namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

  public class ElvishWarrior : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Elvish Warrior")
        .ManaCost("{G}{G}")
        .Type("Creature - Elf Warrior")
        .FlavorText(
          "As graceful as a deer leaping a stream and as deadly as the wolf waiting in ambush on the other side, elvish warriors are the eyes of the forest as well as its unsheathed claws.")
        .Power(2)
        .Toughness(3);
    }
  }
}
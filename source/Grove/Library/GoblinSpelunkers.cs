namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

  public class GoblinSpelunkers : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Goblin Spelunkers")
        .ManaCost("{2}{R}")
        .Type("Creature Goblin Warrior")
        .Text("{Mountainwalk}")
        .FlavorText("'It only short jump. You go first.'{EOL}'AIIIEEEE'{EOL}'Hmm . . . we go different way now.'")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Mountainwalk);
    }
  }
}
namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class Juggernaut : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Juggernaut")
        .ManaCost("{4}")
        .Type("Artifact Creature — Juggernaut")
        .Text("Juggernaut attacks each turn if able.{EOL}Juggernaut can't be blocked by Walls.")
        .FlavorText("Many a besieged city has surrendered upon hearing the distinctive rumble of the juggernaut.")
        .Power(5)
        .Toughness(3)
        .SimpleAbilities(Static.AttacksEachTurnIfAble, Static.CannotBeBlockedByWalls);
    }
  }
}
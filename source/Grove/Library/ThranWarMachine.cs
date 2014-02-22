namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

  public class ThranWarMachine : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thran War Machine")
        .ManaCost("{4}")
        .Type("Artifact Creature Construct")
        .Text("{Echo} {4}{EOL}Thran War Machine attacks each turn if able.")
        .Power(4)
        .Toughness(5)
        .Echo("{4}")
        .SimpleAbilities(Static.AttacksEachTurnIfAble);
    }
  }
}
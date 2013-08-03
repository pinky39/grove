namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class ThranWarMachine : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
namespace Grove.Artifical.Decisions
{
  using System;
  using System.Linq;

  [Serializable]
  public class DeclareBlockers : Gameplay.Decisions.DeclareBlockers
  {
    protected override void ExecuteQuery()
    {
      Result = new BlockStrategy
        (
        Game.Combat.Attackers.Select(x => x.Card),
        Controller.Battlefield.CreaturesThatCanBlock,
        Controller.Life
        );
    }
  }
}
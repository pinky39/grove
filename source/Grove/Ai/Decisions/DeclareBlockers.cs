namespace Grove.Core.Decisions.Machine
{
  using System.Linq;
  using Grove.Core.Ai;

  public class DeclareBlockers : Decisions.DeclareBlockers
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
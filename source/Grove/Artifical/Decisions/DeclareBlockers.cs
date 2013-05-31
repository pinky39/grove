namespace Grove.Artifical.Decisions
{
  using System.Linq;
  using Gameplay.Decisions.Results;

  public class DeclareBlockers : Gameplay.Decisions.DeclareBlockers
  {
    public DeclareBlockers()
    {
      Result = new ChosenBlockers();
    }
    
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
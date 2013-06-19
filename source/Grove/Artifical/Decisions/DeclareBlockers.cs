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
      var p = new BlockStrategyParameters
        {
          Attackers = Combat.Attackers.Select(x => x.Card).ToList(),
          BlockerCandidates = Controller.Battlefield.CreaturesThatCanBlock.ToList(),
          DefendersLife = Controller.Life
        };

      Result = new BlockStrategy(p).ChooseBlockers();
    }
  }
}
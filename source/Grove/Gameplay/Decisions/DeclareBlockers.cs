namespace Grove.Gameplay.Decisions
{
  using Results;

  public abstract class DeclareBlockers : Decision<ChosenBlockers>
  {    
    protected override bool ShouldExecuteQuery
    {
      get
      {
        return Game.Combat.CanAnyAttackerBeBlockedByAny(
          Controller.Battlefield.Creatures);
      }
    }

    protected override void SetResultNoQuery()
    {
      Result = new ChosenBlockers();
    }

    public override void ProcessResults()
    {      
      foreach (var pair in Result)
      {
        Combat.DeclareBlocker(pair.Blocker, pair.Attacker);
      }
    }
  }
}
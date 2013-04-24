namespace Grove.Core.Decisions
{
  using Results;

  public abstract class DeclareBlockers : Decision<ChosenBlockers>
  {
    protected DeclareBlockers()
    {
      Result = NoBlockers();
    }    

    protected override bool ShouldExecuteQuery
    {
      get
      {
        return Game.Combat.CanAnyAttackerBeBlockedByAny(
          Controller.Battlefield.Creatures);
      }
    }

    private static ChosenBlockers NoBlockers()
    {
      return new ChosenBlockers();
    }

    public override void ProcessResults()
    {
      if (Result == null)
        return;

      foreach (var pair in Result)
      {
        Game.Combat.DeclareBlocker(pair.Blocker, pair.Attacker);
      }
    }
  }
}
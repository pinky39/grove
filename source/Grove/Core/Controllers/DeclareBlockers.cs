namespace Grove.Core.Controllers
{
  using Results;

  public abstract class DeclareBlockers : Decision<ChosenBlockers>
  {
    protected DeclareBlockers()
    {
      Result = NoBlockers();
    }

    public Combat Combat { get; set; }

    protected override bool ShouldExecuteQuery
    {
      get
      {
        return Combat.CanAnyAttackerBeBlockedByAny(
          Player.Battlefield.Creatures);
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
        Combat.DeclareBlocker(pair.Blocker, pair.Attacker);
      }
    }
  }
}
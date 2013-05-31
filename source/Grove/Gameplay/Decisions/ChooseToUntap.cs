namespace Grove.Gameplay.Decisions
{
  using Results;

  public abstract class ChooseToUntap : Decision<BooleanResult>
  {
    public Card Permanent { get; set; }

    protected override bool ShouldExecuteQuery { get { return Permanent.IsTapped; } }

    protected override void SetResultNoQuery()
    {
      Result = false;
    }

    public override void ProcessResults()
    {
      if (Result.IsTrue)
      {
        Permanent.Untap();
      }
    }
  }
}
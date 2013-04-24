namespace Grove.Core.Decisions
{
  using Results;

  public abstract class ChooseToUntap : Decision<BooleanResult>
  {
    public Card Permanent { get; set; }

    protected override bool ShouldExecuteQuery
    {
      get { return Permanent.IsTapped; }
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
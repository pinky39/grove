namespace Grove.Core.Decisions.Machine
{
  using Results;

  public class SelectStartingPlayer : Decisions.SelectStartingPlayer
  {
    protected override void ExecuteQuery()
    {
      Result = new ChosenPlayer(Controller);
    }
  }
}
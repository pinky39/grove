namespace Grove.Core.Controllers.Machine
{
  using Results;

  public class SelectStartingPlayer : Controllers.SelectStartingPlayer
  {
    protected override void ExecuteQuery()
    {
      Result = new ChosenPlayer(Controller);
    }
  }
}
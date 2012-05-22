namespace Grove.Core.Controllers.Machine
{
  public class SelectStartingPlayer : Controllers.SelectStartingPlayer
  {
    protected override void ExecuteQuery()
    {
      Result = Player;
    }
  }
}
namespace Grove.Ai.Decisions
{
  using Gameplay.Decisions.Results;

  public class SelectStartingPlayer : Gameplay.Decisions.SelectStartingPlayer
  {
    protected override void ExecuteQuery()
    {
      Result = new ChosenPlayer(Controller);
    }
  }
}
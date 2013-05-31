namespace Grove.Artifical.Decisions
{
  using Gameplay.Decisions.Results;

  public class SelectStartingPlayer : Gameplay.Decisions.SelectStartingPlayer
  {
    public SelectStartingPlayer()
    {
      Result = new ChosenPlayer(null);
    }
    
    protected override void ExecuteQuery()
    {
      Result = new ChosenPlayer(Controller);
    }
  }
}
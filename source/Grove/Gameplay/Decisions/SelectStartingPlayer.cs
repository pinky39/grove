namespace Grove.Gameplay.Decisions
{
  using Results;

  public abstract class SelectStartingPlayer : Decision<ChosenPlayer>
  {    
    public override void ProcessResults()
    {
      Game.Players.Starting = Result.Player;
    }
  }
}
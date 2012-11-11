namespace Grove.Core.Controllers
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
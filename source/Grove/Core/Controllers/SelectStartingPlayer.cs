namespace Grove.Core.Controllers
{
  using Results;

  public abstract class SelectStartingPlayer : Decision<ChosenPlayer>
  {
    public Players Players { get; set; }

    public override void ProcessResults()
    {
      Players.Starting = Result.Player;
    }
  }
}
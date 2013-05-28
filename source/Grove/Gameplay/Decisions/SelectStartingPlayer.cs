namespace Grove.Gameplay.Decisions
{
  using System;
  using Results;

  [Serializable]
  public abstract class SelectStartingPlayer : Decision<ChosenPlayer>
  {
    public override void ProcessResults()
    {
      Game.Players.Starting = Result.Player;
    }
  }
}
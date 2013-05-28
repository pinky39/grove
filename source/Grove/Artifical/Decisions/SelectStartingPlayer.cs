namespace Grove.Artifical.Decisions
{
  using System;
  using Gameplay.Decisions.Results;

  [Serializable]
  public class SelectStartingPlayer : Gameplay.Decisions.SelectStartingPlayer
  {
    protected override void ExecuteQuery()
    {
      Result = new ChosenPlayer(Controller);
    }
  }
}
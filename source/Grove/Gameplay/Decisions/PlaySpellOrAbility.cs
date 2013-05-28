namespace Grove.Gameplay.Decisions
{
  using System;
  using Results;

  [Serializable]
  public abstract class PlaySpellOrAbility : Decision<ChosenPlayable>
  {
    public override bool WasPriorityPassed { get { return Result.WasPriorityPassed; } }

    public override void ProcessResults()
    {
      Result.Playable.Play();
    }
  }
}
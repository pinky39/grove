namespace Grove.Gameplay.Decisions
{
  using Results;

  public abstract class PlaySpellOrAbility : Decision<ChosenPlayable>
  {
    public override bool WasPriorityPassed { get { return Result.WasPriorityPassed; } }

    public override void ProcessResults()
    {
      Result.Playable.Play();
    }
  }
}
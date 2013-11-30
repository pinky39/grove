namespace Grove.Gameplay.Decisions
{
  using Results;

  public abstract class PlaySpellOrAbility : Decision<ChosenPlayable>
  {
    public override bool IsPass { get { return Result.WasPriorityPassed; } }

    public override void ProcessResults()
    {      
      Result.Playable.Play();
    }
  }
}
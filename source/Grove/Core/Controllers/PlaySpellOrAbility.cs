namespace Grove.Core.Controllers
{
  using Results;

  public abstract class PlaySpellOrAbility : Decision<ChosenPlayable>
  {        
    public override void ProcessResults()
    {
      Result.Playable.Play();      
    }

    public override bool WasPriorityPassed
    {
      get { return Result.WasPriorityPassed; }
    }
   }
}
namespace Grove.Core.Controllers
{
  using Effects;
  using Results;
  using Zones;

  public abstract class SetTriggeredAbilityTarget : Decision<ChosenTarget>
  {        
    public Effect Effect { get; set; }
    public Stack Stack { get; set; }
    public TargetSelector TargetSelector { get; set; }

    public override void ProcessResults()
    {
      Effect.Target = Result.Target;            
      Stack.Push(Effect);
    }   
  }
}
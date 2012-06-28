namespace Grove.Core.Controllers
{
  using Effects;
  using Results;
  using Zones;

  public abstract class SetTriggeredAbilityTarget : Decision<ChosenTargets>
  {        
    public Effect Effect { get; set; }
    public Stack Stack { get; set; }
    public TargetSelectors TargetSelectors { get; set; }

    public override void ProcessResults()
    {
      if (Result.Targets == null) 
        return;
      
      foreach (var target in Result.Targets)
      {
        Effect.Targets[target.Key]  = target.Value;
      }                        
        
      Stack.Push(Effect);
    }   
  }
}
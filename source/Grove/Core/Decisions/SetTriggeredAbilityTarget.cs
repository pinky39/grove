namespace Grove.Core.Decisions
{
  using Effects;
  using Results;
  using Targeting;

  public abstract class SetTriggeredAbilityTarget : Decision<ChosenTargets>
  {
    public TriggeredAbility Source { get; set; }
    public object TriggerMessage { get; set; }
    public EffectFactory EffectFactory { get; set; }
    public TargetSelector TargetSelector { get; set; }    

    public override void ProcessResults()
    {
      if (!Result.HasTargets)
        return;

      var effectParameters = new EffectParameters
        {
          Source = Source,
          Targets = Result.Targets,
          TriggerMessage = TriggerMessage
        };

      var effect = EffectFactory().Initialize(effectParameters, Game);            
      Stack.Push(effect);
    }
  }
}
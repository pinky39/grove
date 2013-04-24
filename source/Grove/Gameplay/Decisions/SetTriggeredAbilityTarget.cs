namespace Grove.Core.Decisions
{
  using System.Collections.Generic;
  using Ai;
  using Effects;
  using Results;
  using Targeting;

  public abstract class SetTriggeredAbilityTarget : Decision<ChosenTargets>
  {
    public TriggeredAbility Source { get; set; }
    public object TriggerMessage { get; set; }
    public EffectFactory EffectFactory { get; set; }
    public TargetSelector TargetSelector { get; set; }
    public List<MachinePlayRule> MachineRules { get; set; }    
    
    public override void ProcessResults()
    {              
      var effectParameters = new EffectParameters
        {
          Source = Source,
          Targets = Result.Targets,
          TriggerMessage = TriggerMessage
        };

      
      var effect = EffectFactory();
      if (Result.HasTargets == false)
      {
        effect.Initialize(effectParameters, Game, initializeDynamicParameters: false);
        effect.EffectCountered(SpellCounterReason.IllegalTarget);
        return;
      }

      effect.Initialize(effectParameters, Game);
      Stack.Push(effect);
    }
  }
}
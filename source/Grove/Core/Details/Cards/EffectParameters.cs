namespace Grove.Core.Details.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Targeting;

  public delegate void EffectInitializer<TEffect>(EffectCreationContext<TEffect> context) where TEffect : Effect;

  public class EffectParameters
  {
    public EffectParameters(IEffectSource source, ActivationParameters activation = null, object triggerMessage = null, IEnumerable<ITarget> targets = null, IEnumerable<ITarget> costTargets = null)
    {
      TriggerMessage = triggerMessage;
      Source = source;
      Activation = activation ?? ActivationParameters.Default;
      Targets = targets ?? Enumerable.Empty<ITarget>();
      CostTargets = costTargets ?? Enumerable.Empty<ITarget>();
    }

    public object TriggerMessage { get; set; }
    public IEffectSource Source { get; set; }
    public ActivationParameters Activation { get; set; }
    public IEnumerable<ITarget> Targets { get; set; }
    public IEnumerable<ITarget> CostTargets { get; set; }

    public TMessage Trigger<TMessage>()
    {
      return (TMessage) TriggerMessage;
    }
  }
}
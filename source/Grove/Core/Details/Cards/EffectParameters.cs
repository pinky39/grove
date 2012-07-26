namespace Grove.Core.Details.Cards
{
  using Effects;
  using Targeting;

  public delegate void EffectInitializer<TEffect>(EffectCreationContext<TEffect> context) where TEffect : Effect;

  public class EffectParameters
  {
    public EffectParameters(IEffectSource source, ActivationParameters activation = null, object triggerMessage = null,
      Targets targets = null)
    {
      TriggerMessage = triggerMessage;      
      Source = source;
      Activation = activation ?? ActivationParameters.Default;
      Targets = targets ?? new Targets();
    }

    public object TriggerMessage { get; set; }    
    public IEffectSource Source { get; set; }
    public ActivationParameters Activation { get; set; }
    public Targets Targets { get; private set; }

    public TMessage Trigger<TMessage>()
    {
      return (TMessage) TriggerMessage;
    }
  }
}
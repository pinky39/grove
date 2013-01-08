namespace Grove.Core.Cards
{
  using Ai;
  using Effects;
  using Targeting;

  public delegate void EffectInitializer<TEffect>(EffectCreationContext<TEffect> context) where TEffect : Effect;

  public class EffectParameters
  {
    public EffectParameters(IEffectSource source, EffectCategories effectCategories, ActivationParameters activationParameters = null,
      object triggerMessage = null)
    {      
      TriggerMessage = triggerMessage;
      Source = source;
      Activation = activationParameters ?? ActivationParameters.Default;
      Targets = Activation.Targets ?? new Targets();
      EffectCategories = effectCategories;
    }

    public object TriggerMessage { get; set; }
    public IEffectSource Source { get; set; }
    public ActivationParameters Activation { get; set; }
    public Targets Targets { get; private set; }
    public EffectCategories EffectCategories { get; private set; }

    public TMessage Trigger<TMessage>()
    {
      return (TMessage) TriggerMessage;
    }
  }
}
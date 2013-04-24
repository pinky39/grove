namespace Grove.Core
{
  using Effects;
  using Targeting;
  
  public class EffectParameters
  {
    public IEffectSource Source;
    public Targets Targets;
    public object TriggerMessage;
    public int? X;
  }
}
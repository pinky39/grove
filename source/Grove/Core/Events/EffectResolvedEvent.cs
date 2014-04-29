namespace Grove.Events
{
  public class EffectResolvedEvent
  {
    public readonly Effect Effect;

    public EffectResolvedEvent(Effect effect)
    {
      Effect = effect;
    }
  }
}
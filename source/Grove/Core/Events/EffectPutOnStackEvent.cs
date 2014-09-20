namespace Grove.Events
{
  public class EffectPutOnStackEvent
  {
    public readonly Effect Effect;

    public EffectPutOnStackEvent(Effect effect)
    {
      Effect = effect;
    }
  }
}
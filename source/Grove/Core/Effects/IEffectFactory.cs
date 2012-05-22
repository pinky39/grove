namespace Grove.Core.Effects
{
  public interface IEffectFactory
  {
    Effect CreateEffect(IEffectSource source, int? x = null, bool wasKickerPaid = false);
  }
}
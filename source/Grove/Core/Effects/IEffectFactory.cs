namespace Grove.Core.Effects
{
  public interface IEffectFactory : IHashable
  {
    Effect CreateEffect(IEffectSource source, int? x = null, bool wasKickerPaid = false, object triggerContext = null);
  }
}
namespace Grove.Core.Details.Cards.Effects
{
  using Infrastructure;

  public interface IEffectFactory : IHashable
  {
    Effect CreateEffect(IEffectSource source, int? x = null, bool wasKickerPaid = false, object triggerContext = null);
  }
}
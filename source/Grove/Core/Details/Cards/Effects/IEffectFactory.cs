namespace Grove.Core.Details.Cards.Effects
{
  using Infrastructure;

  public interface IEffectFactory : IHashable
  {
    Effect CreateEffect(EffectParameters parameters);
  }
}
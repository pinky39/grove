namespace Grove.Core.Effects
{
  using Grove.Infrastructure;

  public interface IEffectFactory : IHashable
  {
    Effect CreateEffect(EffectParameters parameters, Game game);
  }
}
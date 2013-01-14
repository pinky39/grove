namespace Grove.Core.Modifiers
{
  using Grove.Core.Targeting;

  public interface IContinuousEffectFactory
  {
    ContinuousEffect Create(Card source, ITarget target, Game game);
  }
}
namespace Grove.Core.Cards.Modifiers
{
  using Targeting;

  public interface IContinuousEffectFactory
  {
    ContinuousEffect Create(Card source, ITarget target, Game game);
  }
}
namespace Grove.Core.Cards.Modifiers
{
  public interface IContinuousEffectFactory
  {
    ContinuousEffect Create(Card source, Game game);
  }
}
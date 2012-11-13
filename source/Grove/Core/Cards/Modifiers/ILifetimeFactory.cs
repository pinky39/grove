namespace Grove.Core.Cards.Modifiers
{
  public interface ILifetimeFactory
  {        
    Lifetime CreateLifetime(Game game);
  }
}
namespace Grove.Core.Modifiers
{
  public interface ILifetimeFactory
  {        
    Lifetime CreateLifetime(Game game);
  }
}
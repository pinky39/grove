namespace Grove.Core.Details.Cards.Modifiers
{
  public interface ILifetimeFactory
  {        
    Lifetime CreateLifetime(Game game);
  }
}
namespace Grove.Core.Cards.Counters
{
  public interface ICounterFactory
  {
    Counter Create(Game game);
  }
}
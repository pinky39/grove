namespace Grove.Core.Counters
{
  public interface ICounterFactory
  {
    Counter Create(Game game);
  }
}
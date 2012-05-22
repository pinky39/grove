namespace Grove.Core.Counters
{
  using Infrastructure;

  [Copyable]
  public abstract class Counter
  {
    protected ChangeTracker ChangeTracker { get { return Game.ChangeTracker; } }
    protected Game Game { get; set; }
    public virtual void ModifyPower(Power power) {}
    public virtual void ModifyToughness(Toughness toughness) {}

    public abstract void Remove();

    public class Factory<T> : ICounterFactory where T : Counter, new()
    {
      public Game Game { get; set; }
      public Initializer<T> Init { get; set; }

      public Counter Create()
      {
        var counter = new T();
        counter.Game = Game;
        Init(counter, new Creator(Game));

        return counter;
      }
    }
  }
}
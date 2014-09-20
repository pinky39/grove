namespace Grove
{
  using Grove.Infrastructure;

  [Copyable]
  public abstract class Counter : GameObject
  {
    public abstract CounterType Type { get; }
    public virtual void ModifyStrenght(Strenght strenght) { }

    public virtual Counter Initialize(Game game)
    {
      Game = game;
      return this;
    }

    public abstract void Remove();
  }
}
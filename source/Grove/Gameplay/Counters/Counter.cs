namespace Grove.Gameplay.Counters
{
  using Characteristics;
  using Infrastructure;
  using Misc;

  [Copyable]
  public abstract class Counter : GameObject
  {
    public virtual void ModifyPower(Power power) {}
    public virtual void ModifyToughness(Toughness toughness) {}
    public abstract CounterType Type { get; }

    public virtual Counter Initialize(Game game)
    {
      Game = game;
      return this;
    }

    public abstract void Remove();
  }
}
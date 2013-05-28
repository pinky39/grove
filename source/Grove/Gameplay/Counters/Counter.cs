namespace Grove.Gameplay.Counters
{
  using System;
  using Characteristics;
  using Misc;

  [Serializable]
  public abstract class Counter : GameObject
  {
    public abstract CounterType Type { get; }
    public virtual void ModifyPower(Power power) {}
    public virtual void ModifyToughness(Toughness toughness) {}

    public virtual Counter Initialize(Game game)
    {
      Game = game;
      return this;
    }

    public abstract void Remove();
  }
}
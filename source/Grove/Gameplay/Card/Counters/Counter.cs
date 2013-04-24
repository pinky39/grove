﻿namespace Grove.Gameplay.Card.Counters
{
  using Characteristics;
  using Common;
  using Grove.Infrastructure;

  [Copyable]
  public abstract class Counter : GameObject
  {
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
namespace Grove.Core.Effects
{
  using System;
  using Infrastructure;

  [Copyable]
  public class DynamicParameter<T>
  {
    private readonly Func<Effect, T> _getter;

    public T Value { get; private set; }

    public DynamicParameter(Func<Effect, T> getter)
    {
      _getter = getter;
    }

    public DynamicParameter(T value)
    {
      Value = value;
    }

    public T Evaluate(Effect effect)
    {
      if (_getter != null)
        Value = _getter(effect);
            
      return Value;
    }

    public static implicit operator DynamicParameter<T>(Func<Effect,T> getter)
    {
      return new DynamicParameter<T>(getter);
    }

    public static implicit operator DynamicParameter<T>(T value)
    {
      return new DynamicParameter<T>(value);
    }
  }
}
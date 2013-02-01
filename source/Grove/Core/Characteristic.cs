namespace Grove.Core
{
  using System;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public abstract class Characteristic<T> : GameObject
  {
    private readonly T _baseValue;
    private readonly TrackableList<PropertyModifier<T>> _modifiers = new TrackableList<PropertyModifier<T>>();

    protected Characteristic() {}
    protected Characteristic(T value)
    {
      _baseValue = value;

    }
    
    public virtual void Initialize(Game game, IHashDependancy hashDependancy)
    {
      Game = game;
      
      _modifiers.Initialize(game.ChangeTracker, hashDependancy);
    }

    public virtual T Value
    {
      get
      {
        T value = _baseValue;
        foreach (var modifier in _modifiers.OrderBy(x => x.Priority))
        {
          value = modifier.Apply(value);
        }

        return value;
      }
    }

    public void AddModifier(PropertyModifier<T> propertyModifier)
    {
      NotifyIfChanged(() => _modifiers.Add(propertyModifier));
    }

    private void NotifyIfChanged(Action action)
    {
      T previousValue = Value;

      action();

      if (!previousValue.Equals(Value))
      {
        OnCharacteristicChanged();
      }
    }

    protected virtual void OnCharacteristicChanged() {}

    public void RemoveModifier(PropertyModifier<T> propertyModifier)
    {
      NotifyIfChanged(() => _modifiers.Remove(propertyModifier));
    }


    public override string ToString()
    {
      return Value.ToString();
    }
  }
}
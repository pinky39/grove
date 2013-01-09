namespace Grove.Core.Cards
{
  using System;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public abstract class Characteristic<T>
  {
    private readonly T _baseValue;
    private readonly TrackableList<PropertyModifier<T>> _modifiers;

    protected Characteristic() {}

    protected Characteristic(T value, ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _modifiers = new TrackableList<PropertyModifier<T>>(changeTracker, hashDependancy);
      _baseValue = value;
    }

    public virtual T Value
    {
      get
      {
        var value = _baseValue;
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
      var previousValue = Value;

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
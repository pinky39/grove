namespace Grove.Core.Cards
{
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
      _modifiers = new TrackableList<PropertyModifier<T>>(changeTracker);
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
      _modifiers.Add(propertyModifier);
    }

    public void RemoveModifier(PropertyModifier<T> propertyModifier)
    {
      _modifiers.Remove(propertyModifier);
    }


    public override string ToString()
    {
      return Value.ToString();
    }
  }
}
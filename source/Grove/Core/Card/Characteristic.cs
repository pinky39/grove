namespace Grove
{
  using System;
  using System.Linq;
  using Grove.Infrastructure;
  using Modifiers;

  public class Characteristic<T> : GameObject, ICopyContributor
  {
    private readonly T _baseValue;
    private readonly Trackable<T> _currentValue;
    private readonly TrackableList<PropertyModifier<T>> _modifiers = new TrackableList<PropertyModifier<T>>();

    protected Characteristic() {}

    public Characteristic(T value)
    {
      _baseValue = value;
      _currentValue = new Trackable<T>(value);
    }

    public virtual T Value { get { return _currentValue.Value; } private set { _currentValue.Value = value; } }

    public void AfterMemberCopy(object original)
    {
      foreach (var modifier in _modifiers)
      {
        modifier.Changed += OnModifierChanged;
      }
    }

    public virtual void Initialize(Game game, IHashDependancy hashDependancy)
    {
      Game = game;

      _modifiers.Initialize(game.ChangeTracker);
      _currentValue.Initialize(game.ChangeTracker, hashDependancy);
    }

    public void AddModifier(PropertyModifier<T> propertyModifier)
    {
      _modifiers.Add(propertyModifier);
      propertyModifier.Changed += OnModifierChanged;
      UpdateValue();
    }

    private void OnModifierChanged(object sender, EventArgs args)
    {
      UpdateValue();
    }

    private void UpdateValue()
    {
      var value = _baseValue;
      foreach (var modifier in _modifiers.OrderBy(x => x.Priority))
      {
        value = modifier.Apply(value);
      }

      if (!Value.Equals(value))
      {
        Value = value;
        OnCharacteristicChanged(value, Value);
      }
    }

    protected virtual void OnCharacteristicChanged(T oldValue, T newValue)
    {
      
    }

    public void RemoveModifier(PropertyModifier<T> propertyModifier)
    {
      _modifiers.Remove(propertyModifier);
      propertyModifier.Changed -= OnModifierChanged;
      UpdateValue();
    }

    public override string ToString()
    {
      return Value.ToString();
    }
  }
}
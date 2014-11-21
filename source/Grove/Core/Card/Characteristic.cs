namespace Grove
{
  using System.Linq;
  using Infrastructure;
  using Modifiers;

  public class CharacteristicChangedParams<TV>
  {
    public readonly TV NewValue;
    public readonly TV OldValue;

    public CharacteristicChangedParams(TV oldValue, TV newValue)
    {
      OldValue = oldValue;
      NewValue = newValue;
    }
  }

  public class Characteristic<T> : GameObject, ICopyContributor
  {
    private readonly Trackable<T> _baseValue;
    private readonly Trackable<T> _currentValue;
    private readonly TrackableList<PropertyModifier<T>> _modifiers = new TrackableList<PropertyModifier<T>>();
    public TrackableEvent<CharacteristicChangedParams<T>> Changed = new TrackableEvent<CharacteristicChangedParams<T>>();

    protected Characteristic() {}

    public Characteristic(T value)
    {
      _baseValue = new Trackable<T>(value);
      _currentValue = new Trackable<T>(value);
    }

    public virtual T Value { get { return _currentValue.Value; } private set { _currentValue.Value = value; } }

    void ICopyContributor.AfterMemberCopy(object original)
    {
      foreach (var modifier in _modifiers)
      {
        modifier.Changed += OnModifierChanged;
      }

      AfterMemberCopy();
    }

    public void ChangeBaseValue(T newBaseValue)
    {
      _baseValue.Value = newBaseValue;
      UpdateValue();
    }

    public virtual void Initialize(Game game, IHashDependancy hashDependancy)
    {
      Game = game;

      _modifiers.Initialize(ChangeTracker);
      _baseValue.Initialize(ChangeTracker);
      _currentValue.Initialize(ChangeTracker, hashDependancy);
      Changed.Initialize(ChangeTracker);
    }

    public void AddModifier(PropertyModifier<T> propertyModifier)
    {
      _modifiers.Add(propertyModifier);
      propertyModifier.Changed += OnModifierChanged;
      UpdateValue();
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

    protected virtual void AfterMemberCopy() {}

    private void OnModifierChanged()
    {
      UpdateValue();
    }

    private void UpdateValue()
    {
      var oldValue = _currentValue.Value;
      var newValue = _baseValue.Value;
      
      foreach (var modifier in _modifiers.OrderBy(x => x.Priority))
      {
        newValue = modifier.Apply(newValue);
      }

      if (!oldValue.Equals(newValue))
      {        
        Value = newValue;

        Changed.Raise(new CharacteristicChangedParams<T>(oldValue, newValue));
        OnCharacteristicChanged(oldValue, newValue);        
      }
    }

    protected virtual void OnCharacteristicChanged(T oldValue, T newValue) {}
  }
}
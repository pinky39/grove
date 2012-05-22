namespace Grove.Core
{
  using System;
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public abstract class Characteristic<T> : ICopyContributor
  {
    private readonly T _baseValue;
    private readonly Trackable<T> _currentValue;
    private readonly TrackableList<PropertyModifier<T>> _modifiers;

    protected Characteristic()
    {      
    }
    
    protected Characteristic(T value, ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _modifiers = new TrackableList<PropertyModifier<T>>(changeTracker);
      _currentValue = new Trackable<T>(changeTracker, hashDependancy);
      _baseValue = value;
      _currentValue.Value = value;
    }

    public virtual T Value
    {
      get { return _currentValue.Value; }
      protected set { _currentValue.Value = value; }
    }
    
    public void AddModifier(PropertyModifier<T> propertyModifier)
    {      
      _modifiers.Add(propertyModifier);
      propertyModifier.Changed += UpdateValue;
      
      UpdateValue();
    }

    private void UpdateValue(object sender, EventArgs e)
    {
      UpdateValue();
    }

    public void RemoveModifier(PropertyModifier<T> propertyModifier)
    {      
      _modifiers.Remove(propertyModifier);
      propertyModifier.Changed -= UpdateValue;

      UpdateValue();
    }
        
    private void UpdateValue()
    {
      var value = _baseValue;
      foreach (var modifier in _modifiers.OrderBy(x => x.Priority))
      {
        value = modifier.Apply(value);
      }

      Value = value;
    }

    public override string ToString()
    {
      return Value.ToString();
    }

    void ICopyContributor.AfterMemberCopy(object original)
    {
      foreach (var modifier in _modifiers)
      {
        modifier.Changed += UpdateValue;
      }
    }
  }
}
namespace Grove.Core
{
  using System.Collections.Generic;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class ContiniousEffects : IModifiable
  {
    private readonly TrackableList<ContinuousEffect> _continiousEffects;

    private ContiniousEffects() {}

    public ContiniousEffects(ChangeTracker changeTracker)
    {
      _continiousEffects = new TrackableList<ContinuousEffect>(changeTracker);
    }

    public ContiniousEffects(IEnumerable<ContinuousEffect> effects, ChangeTracker changeTracker,
      IHashDependancy hashDependancy)
    {
      _continiousEffects = new TrackableList<ContinuousEffect>(effects, changeTracker, hashDependancy);
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Add(ContinuousEffect effect)
    {
      _continiousEffects.Add(effect);
    }

    public void Remove(ContinuousEffect effect)
    {
      _continiousEffects.Remove(effect);
      effect.Deactivate();
    }

    public void Activate()
    {
      foreach (ContinuousEffect continiousEffect in _continiousEffects)
      {
        continiousEffect.Activate();
      }
    }

    public void Deactivate()
    {
      foreach (ContinuousEffect continiousEffect in _continiousEffects)
      {
        continiousEffect.Deactivate();
      }
    }
  }
}
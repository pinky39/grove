namespace Grove.Core.Cards
{
  using System.Collections.Generic;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class ContiniousEffects : IModifiable, IHashable
  {
    private readonly TrackableList<ContinuousEffect> _continiousEffects;

    private ContiniousEffects() {}

    public ContiniousEffects(ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _continiousEffects = new TrackableList<ContinuousEffect>(changeTracker, hashDependancy);
    }

    public ContiniousEffects(IEnumerable<ContinuousEffect> effects, ChangeTracker changeTracker,
      IHashDependancy hashDependancy)
    {
      _continiousEffects = new TrackableList<ContinuousEffect>(effects, changeTracker, hashDependancy);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_continiousEffects);
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
      foreach (var continiousEffect in _continiousEffects)
      {
        continiousEffect.Activate();
      }
    }

    public void Deactivate()
    {
      foreach (var continiousEffect in _continiousEffects)
      {
        continiousEffect.Deactivate();
      }
    }
  }
}
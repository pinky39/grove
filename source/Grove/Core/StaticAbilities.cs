namespace Grove.Core
{
  using System;
  using System.Linq;
  using System.Collections.Generic;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class StaticAbilities : IStaticAbilities, IModifiable, IHashable
  {
    private readonly ChangeTracker _changeTracker;
    private readonly TrackableList<StaticAbility> _abilities;    

    private StaticAbilities() {}

    public StaticAbilities(IEnumerable<Static> staticAbilities, ChangeTracker changeTracker,
                           IHashDependancy hashDependancy)
    {
      _changeTracker = changeTracker;
      _abilities = new TrackableList<StaticAbility>(
        staticAbilities.Select(x => new StaticAbility(x, changeTracker)) , changeTracker, hashDependancy);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_abilities);
    }

    public bool Deathtouch { get { return Has(Static.Deathtouch); } }
    public bool Defender { get { return Has(Static.Defender); } }
    public bool Fear { get { return Has(Static.Fear); } }
    public bool Flying { get { return Has(Static.Flying); } }
    public bool Haste { get { return Has(Static.Haste); } }
    public bool Hexproof { get { return Has(Static.Hexproof); } }
    public bool Indestructible { get { return Has(Static.Indestructible); } }
    public bool Lifelink { get { return Has(Static.Lifelink); } }
    public bool Shroud { get { return Has(Static.Shroud); } }
    public bool Trample { get { return Has(Static.Trample); } }
    public bool Unblockable { get { return Has(Static.Unblockable); } }
    public bool FirstStrike { get { return Has(Static.FirstStrike); } }
    public bool DoubleStrike { get { return Has(Static.DoubleStrike); } }
    public bool Reach { get { return Has(Static.Reach); } }
    public bool Vigilance { get { return Has(Static.Vigilance); } }
    public bool Swampwalk { get { return Has(Static.Swampwalk);} }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Add(Static ability)
    {
      _abilities.Add(new StaticAbility(ability, _changeTracker));
    }

    public void Remove(Static ability)
    {
      var staticAbility = _abilities
        .Where(x => x.Value == ability)
        .OrderBy(x => x.IsEnabled ? 0 : 1)
        .FirstOrDefault();

      if (staticAbility != null)      
        _abilities.Remove(staticAbility);
    }

    private bool Has(Static ability)
    {
      return _abilities.Any(x => x.IsEnabled && x.Value == ability);
    }

    public void Disable()
    {
      foreach (var staticAbility in _abilities)
      {
        staticAbility.Disable();
      }
    }

    public void Enable()
    {
      foreach (var staticAbility in _abilities)
      {
        staticAbility.Enable();
      }
    }
  }
}
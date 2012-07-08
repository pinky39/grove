namespace Grove.Core.Preventions
{
  using System.Linq;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class DamagePreventions : IModifiable, IHashable
  {
    private readonly TrackableList<DamagePrevention> _preventions;

    private DamagePreventions() {}

    public DamagePreventions(ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _preventions = new TrackableList<DamagePrevention>(changeTracker, hashDependancy);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_preventions);
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Add(DamagePrevention prevention)
    {
      _preventions.Add(prevention);
    }

    public int PreventDamage(Card damageSource, int amount, bool queryOnly = false)
    {
      int damageLeft = amount;

      foreach (DamagePrevention preventionEffect in _preventions.ToList())
      {
        damageLeft = preventionEffect.PreventDamage(damageSource, damageLeft, queryOnly);

        if (damageLeft == 0)
          break;
      }

      return damageLeft;
    }

    public void Remove(DamagePrevention preventaion)
    {
      _preventions.Remove(preventaion);
    }
  }
}
namespace Grove.Core.Cards.Redirections
{
  using System.Linq;
  using Grove.Infrastructure;
  using Modifiers;

  [Copyable]
  public class DamageRedirections : IModifiable, IHashable
  {
    private readonly TrackableList<DamageRedirection> _redirections;

    private DamageRedirections() {}

    public DamageRedirections(ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _redirections = new TrackableList<DamageRedirection>(changeTracker, hashDependancy);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_redirections);
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Add(DamageRedirection prevention)
    {
      _redirections.Add(prevention);
    }

    public bool RedirectDamage(Damage damage)
    {
      foreach (var preventionEffect in _redirections.ToList())
      {
        if (preventionEffect.RedirectDamage(damage))
        {
          return true;
        }
      }

      return false;
    }

    public void Remove(DamageRedirection preventaion)
    {
      _redirections.Remove(preventaion);
    }
  }
}
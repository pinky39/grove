namespace Grove.Gameplay.DamageHandling
{
  using System.Linq;
  using Infrastructure;
  using Modifiers;
  using Targeting;

  [Copyable]
  public class DamageRedirections : IAcceptsGameModifier, IHashable
  {
    private readonly TrackableList<DamageRedirection> _redirections = new TrackableList<DamageRedirection>();

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_redirections);
    }

    public void Accept(IGameModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Initialize(ChangeTracker changeTracker)
    {
      _redirections.Initialize(changeTracker);
    }

    public void Add(DamageRedirection prevention)
    {
      _redirections.Add(prevention);
    }

    public bool RedirectDamage(Damage damage, ITarget target)
    {
      foreach (var redirection in _redirections.ToList())
      {
        if (redirection.RedirectDamage(damage, target))
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
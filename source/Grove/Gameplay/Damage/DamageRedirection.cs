namespace Grove.Gameplay.Damage
{
  using System;
  using Infrastructure;
  using Misc;
  using Modifiers;

  [Serializable]
  public abstract class DamageRedirection : GameObject, IHashable
  {
    public Modifier Modifier { get; private set; }

    public int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    public bool RedirectDamage(Damage damage)
    {
      if (damage.WasAlreadyRedirected(this))
        return false;

      var wasRedirected = Redirect(damage);

      if (wasRedirected)
        damage.AddRedirection(this);

      return wasRedirected;
    }

    public virtual void Initialize(Modifier modifier, Game game)
    {
      Modifier = modifier;
      Game = game;
    }

    protected abstract bool Redirect(Damage damage);
  }
}
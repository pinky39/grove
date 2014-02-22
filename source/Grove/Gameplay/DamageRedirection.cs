namespace Grove.Gameplay
{
  using Grove.Infrastructure;
  using Grove.Gameplay.Modifiers;

  [Copyable]
  public abstract class DamageRedirection : GameObject, IHashable
  {
    public Modifier Modifier { get; private set; }

    public abstract int CalculateHash(HashCalculator calc);    

    public bool RedirectDamage(Damage damage, ITarget target)
    {
      if (damage.WasAlreadyRedirected(this))
        return false;

      var wasRedirected = Redirect(damage, target);

      if (wasRedirected)
        damage.AddRedirection(this);

      return wasRedirected;
    }

    public virtual void Initialize(Modifier modifier, Game game)
    {
      Modifier = modifier;
      Game = game;
    }

    protected abstract bool Redirect(Damage damage, ITarget target);
  }
}
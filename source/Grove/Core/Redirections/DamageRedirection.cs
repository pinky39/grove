namespace Grove.Core.Redirections
{
  using Grove.Core.Dsl;
  using Grove.Infrastructure;
  using Grove.Core.Targeting;

  [Copyable]
  public abstract class DamageRedirection : IHashable
  {
    protected ITarget Owner { get; private set; }
    protected Game Game { get; private set; }

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

    protected abstract bool Redirect(Damage damage);

    public class Factory<T> : IDamageRedirectionFactory where T : DamageRedirection, new()
    {      
      public bool OnlyOnce { get; set; }
      public Initializer<T> Init { get; set; }

      public DamageRedirection Create(ITarget owner, Game game)
      {
        var prevention = new T();

        prevention.Owner = owner;
        prevention.Game = game;

        Init(prevention);

        return prevention;
      }
    }
  }
}
namespace Grove.Core.Details.Cards.Redirections
{
  using Dsl;
  using Infrastructure;
  using Targeting;

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
      public Game Game { get; set; }
      public bool OnlyOnce { get; set; }
      public Initializer<T> Init { get; set; }

      public DamageRedirection Create(ITarget owner)
      {
        var prevention = new T();

        prevention.Owner = owner;
        prevention.Game = Game;

        Init(prevention, new CardBuilder(Game));

        return prevention;
      }
    }
  }
}
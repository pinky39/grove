namespace Grove.Core.Redirections
{
  using System;
  using CardDsl;
  using Infrastructure;

  [Copyable]
  public abstract class DamageRedirection : IHashable
  {    
    protected ITarget Owner { get; private set; }
    protected Game Game { get; private set; }

     public int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }
    
    public abstract int RedirectDamage(Card damageDealer, int damageAmount, bool queryOnly);

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

        Init(prevention, new CardCreationContext(Game));

        return prevention;
      }
    }
  }
}
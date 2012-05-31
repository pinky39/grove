namespace Grove.Core.DamagePrevention
{
  using Infrastructure;

  [Copyable]
  public abstract class DamagePrevention : IHashable
  {
    protected Card Card { get; private set; }
    protected Game Game { get; private set; }

    public int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    public abstract int PreventDamage(Card damageDealer, int damageAmount);

    public class Factory<T> : IDamagePreventionFactory where T : DamagePrevention, new()
    {
      public Game Game { get; set; }
      public Initializer<T> Init { get; set; }

      public DamagePrevention Create(Card card)
      {
        var prevention = new T();

        prevention.Card = card;
        prevention.Game = Game;

        Init(prevention, new Creator(Game));

        return prevention;
      }
    }
  }
}
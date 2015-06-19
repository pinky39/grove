namespace Grove
{
  using Infrastructure;

  public abstract class DamagePrevention : GameObject, IHashable
  {
    public virtual int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    protected Modifier Modifier { get; private set; }

    public virtual void Initialize(Modifier modifier, Game game)
    {
      Game = game;
      Modifier = modifier;

      Initialize();
    }

    protected virtual void Initialize() {}

    public virtual int PreventDamage(PreventDamageParameters p)
    {
      return 0;
    }

    public virtual int PreventLifeloss(int amount, Player player, bool queryOnly)
    {
      return 0;
    }

    protected Context Ctx { get { return new Context(this, Game); } }

    public class Context
    {
      private readonly DamagePrevention _damagePrevention;
      private readonly Game _game;

      public Context(DamagePrevention damagePrevention, Game game)
      {
        _damagePrevention = damagePrevention;
        _game = game;
      }

      public Card SourceCard
      {
        get { return _damagePrevention.Modifier.SourceCard; }
      }            
    }
  }
}
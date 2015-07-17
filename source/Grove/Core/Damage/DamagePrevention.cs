namespace Grove
{
  using Infrastructure;

  public abstract class DamagePrevention : GameObject, IHashable
  {
    protected Player Controller;
    
    public virtual int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    protected Modifier Modifier { get; private set; }

    public virtual void Initialize(Modifier modifier, Game game)
    {
      Game = game;
      Modifier = modifier;
      Controller = modifier.SourceCard.Controller;

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

    protected Context Ctx(PreventDamageParameters p)
    {
      return new Context(p, this, Game);
    }

    public class Context
    {
      private readonly PreventDamageParameters _pdp;
      private readonly DamagePrevention _damagePrevention;
      private readonly Game _game;

      public int AmountDealt
      {
        get { return _pdp.Amount; }
      }

      public Player You { get { return _damagePrevention.Controller; } }

      public Context(PreventDamageParameters pdp, DamagePrevention damagePrevention, Game game)
      {
        _pdp = pdp;
        _damagePrevention = damagePrevention;
        _game = game;
      }            
    }
  }
}
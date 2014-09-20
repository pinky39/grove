namespace Grove
{
  using Grove.Infrastructure;

  public abstract class DamagePrevention : GameObject, IHashable
  {
    public abstract int CalculateHash(HashCalculator calc);
        

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
  }
}
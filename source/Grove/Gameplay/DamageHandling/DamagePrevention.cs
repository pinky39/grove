namespace Grove.Gameplay.DamageHandling
{
  using Infrastructure;
  using Misc;
  using Modifiers;

  public abstract class DamagePrevention : GameObject, IHashable, ILifetimeDependency
  {
    protected DamagePrevention()
    {
      EndOfLife = new TrackableEvent(this);
    }    

    public int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    public TrackableEvent EndOfLife { get; set; }

    protected Modifier Modifier { get; private set; }

    public virtual void Initialize(Modifier modifier, Game game)
    {
      Game = game;
      Modifier = modifier;
      EndOfLife.Initialize(game.ChangeTracker);

      Initialize();
    }

    protected virtual void Initialize() {}

    public virtual int PreventDamage(PreventDamageParameters parameters)
    {
      return 0;
    }

    public virtual int PreventLifeloss(int amount, Player player, bool queryOnly)
    {
      return 0;
    }
  }
}
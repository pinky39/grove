namespace Grove.Core.Costs
{
  using System.Linq;
  using Infrastructure;
  using Mana;
  using Targeting;

  public abstract class Cost : GameObject, IHashable
  {
    protected Card Card { get; private set; }
    protected TargetValidator Validator { get; set; }

    public virtual int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    public bool CanPay()
    {
      int? maxX = null;
      return CanPay(ref maxX);
    }

    public abstract bool CanPay(ref int? maxX);

    public virtual void Pay(Targets targets, int? x)
    {
      // only one cost target is currently 
      // supported, changes this to support
      // more
      Pay(targets.Cost.FirstOrDefault(), x);
    }

    protected virtual void Pay(ITarget target, int? x) {}

    public virtual IManaAmount GetManaCost()
    {
      return ManaAmount.Zero;
    }

    public virtual void Initialize(Card card, Game game)
    {
      Game = game;
      Card = card;
    }
  }
}
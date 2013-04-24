namespace Grove.Gameplay.Card.Costs
{
  using System.Linq;
  using Common;
  using Grove.Infrastructure;
  using Mana;
  using Targeting;

  public abstract class Cost : GameObject, IHashable
  {
    protected Card Card { get; private set; }
    protected TargetValidator Validator { get; set; }

    public virtual bool HasX { get { return false; } }

    public virtual int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    protected abstract void CanPay(CanPayResult result);

    public CanPayResult CanPay()
    {
      var result = new CanPayResult();
      CanPay(result);

      return result;
    }

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
      return Mana.Zero;
    }

    public virtual void Initialize(Card card, Game game, TargetValidator validator = null)
    {
      Game = game;
      Card = card;
      Validator = validator;
    }
  }
}
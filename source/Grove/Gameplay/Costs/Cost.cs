namespace Grove.Gameplay.Costs
{
  using Infrastructure;

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

    public virtual void Pay(Targets targets, int? x, int repeat = 1)
    {
      // only one cost target is currently 
      // supported, changes this to support
      // more
      PayCost(targets, x, repeat);
    }

    protected virtual void PayCost(Targets targets, int? x, int repeat) {}

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
namespace Grove.Costs
{
  using Infrastructure;

  public abstract class Cost : GameObject, IHashable
  {
    protected Card Card { get; private set; }
    protected TargetValidator Validator { get; set; }

    public virtual bool HasX
    {
      get { return false; }
    }

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

    public abstract void Pay(PayCostParameters p);    

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
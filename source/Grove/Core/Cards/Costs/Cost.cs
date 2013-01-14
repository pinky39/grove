namespace Grove.Core.Cards.Costs
{
  using Ai;
  using Dsl;
  using Infrastructure;
  using Mana;
  using Targeting;

  [Copyable]
  public abstract class Cost : IHashable
  {
    public Card Card { get; private set; }
    public Player Controller { get { return Card.Controller; } }
    public Game Game { get; private set; }
    protected TargetValidator Validator { get; private set; }

    public CalculateX XCalculator { get; set; }
    protected bool HasX {get { return XCalculator != null; }}

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
    public abstract void Pay(ITarget target, int? x);

    protected virtual void AfterInit() {}

    public virtual IManaAmount GetManaCost()
    {
      return ManaAmount.Zero;
    }

    public class Factory<TCost> : ICostFactory where TCost : Cost, new()
    {
      public Initializer<TCost> Init = delegate { };

      public Cost CreateCost(Card card, TargetValidator validator, Game game)
      {
        var cost = new TCost();
        cost.Card = card;
        cost.Game = game;
        cost.Validator = validator;

        Init(cost);
        cost.AfterInit();

        return cost;
      }
    }
  }
}
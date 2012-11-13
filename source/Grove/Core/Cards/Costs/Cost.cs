namespace Grove.Core.Cards.Costs
{
  using Grove.Core.Ai;
  using Grove.Core.Dsl;
  using Grove.Infrastructure;
  using Grove.Core.Targeting;

  [Copyable]
  public abstract class Cost : IHashable
  {
    protected Card Card { get; private set; }
    protected Player Controller { get { return Card.Controller; } }
    protected Game Game { get; private set; }
    public TargetValidator Validator { get; private set;  }

    public CalculateX XCalculator { get; set; }

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
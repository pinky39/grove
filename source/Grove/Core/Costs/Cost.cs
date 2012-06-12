namespace Grove.Core.Costs
{
  using Ai;
  using CardDsl;
  using Infrastructure;

  [Copyable]
  public abstract class Cost : IHashable
  {
    protected ActivatedAbility Ability { get; private set; }
    protected Card Card { get { return Ability.OwningCard; } }
    protected Player Controller { get { return Card.Controller; } }
    protected Game Game { get; private set; }
    public TargetSelector TargetSelector { get; private set; }

    public CalculateX XCalculator { get; set; }

    public virtual int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    public abstract bool CanPay(ref int? maxX);
    public abstract void Pay(ITarget target, int? x);

    public void SetTargetSelector(ITargetSelectorFactory factory)
    {
      TargetSelector = factory.Create(Ability.OwningCard);
    }

    protected virtual void AfterInit() {}

    public class Factory<TCost> : ICostFactory where TCost : Cost, new()
    {
      public Initializer<TCost> Init = delegate { };
      public Game Game { get; set; }

      public Cost CreateCost(ActivatedAbility ability)
      {
        var cost = new TCost();
        cost.Ability = ability;
        cost.Game = Game;

        Init(cost, new CardCreationCtx(Game));
        cost.AfterInit();

        return cost;
      }
    }
  }
}
namespace Grove.Costs
{
  using Infrastructure;

  public abstract class Cost : GameObject, IHashable
  {
    protected Card Card { get; private set; }
    protected Player Controller { get { return Card.Controller; } }
    protected TargetValidator Validator;
    protected CostType Type { get; private set; }

    public virtual bool HasX { get { return false; } }

    public virtual int CalculateHash(HashCalculator calc)
    {
      return GetType().GetHashCode();
    }

    public virtual CanPayResult CanPay(bool payManaCost)
    {
      var result = CanPayPartial(payManaCost);

      if (result.CanPay && CanPayAdditionalCost())
        return result;

      return false;
    }

    public abstract CanPayResult CanPayPartial(bool needsToPayManaCost);
    public abstract void PayPartial(PayCostParameters p);

    public virtual void Pay(PayCostParameters p)
    {
      PayAdditionalCost();
      PayPartial(p);
    }

    protected void PayAdditionalCost()
    {
      var change = Game.GetCostChange(Type, Card);
      if (change <= 0)
        return;

      Controller.Consume(change.Colorless(), GetManaUsage());
    }

    protected bool CanPayAdditionalCost()
    {
      var change = Game.GetCostChange(Type, Card);

      var manaUsage = GetManaUsage();

      if (change > 0 && !Controller.HasMana(change, manaUsage))
      {
        return false;
      }

      return true;
    }

    protected ManaUsage GetManaUsage()
    {
      var manaUsage = Type == CostType.Ability
        ? ManaUsage.Abilities
        : ManaUsage.Spells;
      return manaUsage;
    }

    public virtual ManaAmount GetManaCost()
    {
      var change = Game.GetCostChange(Type, Card);
      return change <= 0 ? Mana.Zero : change.Colorless();
    }

    public virtual void Initialize(CostType type, Card card, Game game, TargetValidator validator = null)
    {
      Game = game;
      Card = card;
      Validator = validator;
      Type = type;
    }
  }
}
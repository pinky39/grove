namespace Grove
{
  using System;
  using Grove.Infrastructure;

  public abstract class ManaOutput : GameObject
  {
    public Action<ManaAmount> Decreased = delegate { };
    public Action<ManaAmount> Increased = delegate { };

    protected ManaAbility ManaAbility;
    private Trackable<ManaAmount> _additionalOutput = new Trackable<ManaAmount>(new ZeroManaAmount());

    public virtual ManaAmount GetAmount()
    {
      var amount = GetAmountInternal();
      return amount.Add(_additionalOutput.Value);
    }

    protected abstract ManaAmount GetAmountInternal();

    public void AddAditional(ManaAmount amount)
    {
      _additionalOutput.Value = _additionalOutput.Value.Add(amount);
    }

    public void RemoveAdditional(ManaAmount amount)
    {
      _additionalOutput.Value = _additionalOutput.Value.Remove(amount);
    }

    public void Initialize(ManaAbility manaAbility, Game game)
    {
      Game = game;
      ManaAbility = manaAbility;
      _additionalOutput.Initialize(ChangeTracker);
    }
  }
}
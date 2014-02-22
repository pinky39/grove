namespace Grove.Gameplay
{
  using System;
  using Grove.Infrastructure;

  public abstract class ManaOutput : GameObject
  {
    public Action<IManaAmount> Decreased = delegate { };
    public Action<IManaAmount> Increased = delegate { };

    protected ManaAbility ManaAbility;
    private Trackable<IManaAmount> _additionalOutput = new Trackable<IManaAmount>(new ZeroManaAmount());

    public virtual IManaAmount GetAmount()
    {
      var amount = GetAmountInternal();
      return amount.Add(_additionalOutput.Value);
    }

    protected abstract IManaAmount GetAmountInternal();

    public void AddAditional(IManaAmount amount)
    {
      _additionalOutput.Value = _additionalOutput.Value.Add(amount);
    }

    public void RemoveAdditional(IManaAmount amount)
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
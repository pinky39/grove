namespace Grove.Gameplay.ManaHandling
{
  using System;
  using Abilities;
  using Misc;

  [Serializable]
  public abstract class ManaOutput : GameObject
  {
    public Action<IManaAmount> Decreased = delegate { };
    public Action<IManaAmount> Increased = delegate { };

    protected ManaAbility ManaAbility;
    private IManaAmount _additionalOutput = Mana.Zero;

    public virtual IManaAmount GetAmount()
    {
      var amount = GetAmountInternal();
      return amount.Add(_additionalOutput);
    }

    protected abstract IManaAmount GetAmountInternal();

    public void AddAditional(IManaAmount amount)
    {
      _additionalOutput = _additionalOutput.Add(amount);
    }

    public void RemoveAdditional(IManaAmount amount)
    {
      _additionalOutput = _additionalOutput.Remove(amount);
    }

    public void Initialize(ManaAbility manaAbility, Game game)
    {
      Game = game;
      ManaAbility = manaAbility;
    }
  }
}
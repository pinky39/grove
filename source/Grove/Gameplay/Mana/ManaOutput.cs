namespace Grove.Gameplay.Mana
{
  using System;
  using Card.Abilities;
  using Common;

  public abstract class ManaOutput : GameObject
  {
    private IManaAmount _additionalOutput = Mana.Zero;
    public Action<IManaAmount> Increased = delegate { };
    public Action<IManaAmount> Decreased = delegate { };
    
    protected ManaAbility ManaAbility;    
    
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
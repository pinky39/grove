namespace Grove.Core.Modifiers
{
  using Grove.Core.Mana;

  public class IncreaseManaOutput : Modifier
  {
    private ActivatedAbilities _abilities;
    private ManaAmountIncrement _increment;

    public IManaAmount Amount { get; set; }

    public override void Apply(ActivatedAbilities abilities)
    {
      _abilities = abilities;
      _increment = new ManaAmountIncrement(Amount, Game.ChangeTracker);

      foreach (var manaAbility in _abilities.ManaAbilities)
      {
        manaAbility.AddManaModifier(_increment);
      }       
    }
    
    protected override void Unapply()
    {
      foreach (var manaAbility in _abilities.ManaAbilities)
      {
        manaAbility.RemoveManaModifier(_increment);
      }       
    }
  }
}
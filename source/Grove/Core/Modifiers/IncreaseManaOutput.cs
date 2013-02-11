namespace Grove.Core.Modifiers
{
  using Mana;

  public class IncreaseManaOutput : Modifier
  {
    private readonly IManaAmount _amount;
    private ActivatedAbilities _abilities;
    private ManaAmountIncrement _increment;

    private IncreaseManaOutput() {}

    public IncreaseManaOutput(IManaAmount amount)
    {
      _amount = amount;
    }

    public override void Apply(ActivatedAbilities abilities)
    {
      _abilities = abilities;
      _increment = new ManaAmountIncrement(_amount, Game.ChangeTracker);

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
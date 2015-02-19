namespace Grove.Modifiers
{
  public class IncreaseManaOutput : Modifier, ICardModifier
  {
    private readonly ManaAmount _amount;
    private ActivatedAbilities _abilities;

    private IncreaseManaOutput() {}

    public IncreaseManaOutput(ManaAmount amount)
    {
      _amount = amount;
    }

    public override void Apply(ActivatedAbilities abilities)
    {
      _abilities = abilities;

      foreach (var manaAbility in _abilities.GetManaAbilities())
      {
        manaAbility.AddAditionalAmountAbilityWillProduce(_amount);
      }
    }

    protected override void Unapply()
    {
      foreach (var manaAbility in _abilities.GetManaAbilities())
      {
        manaAbility.RemoveAdditionalAmountAbilityWillProduce(_amount);
      }
    }
  }
}
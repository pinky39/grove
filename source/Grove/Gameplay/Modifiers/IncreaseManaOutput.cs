namespace Grove.Gameplay.Modifiers
{
  using Abilities;
  using ManaHandling;

  public class IncreaseManaOutput : Modifier, ICardModifier
  {
    private readonly IManaAmount _amount;
    private ActivatedAbilities _abilities;

    private IncreaseManaOutput() {}

    public IncreaseManaOutput(IManaAmount amount)
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
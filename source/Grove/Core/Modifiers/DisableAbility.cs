namespace Grove.Modifiers
{
  public class DisableAbility : Modifier, ICardModifier
  {
    private readonly Static _simpleAbility;
    private SimpleAbilities _simpleAbilties;

    private DisableAbility() {}

    public DisableAbility(Static simpleAbility)
    {
      _simpleAbility = simpleAbility;
    }

    public override void Apply(SimpleAbilities abilities)
    {
      abilities.Disable(_simpleAbility);
      _simpleAbilties = abilities;
    }


    protected override void Unapply()
    {
      _simpleAbilties.Enable(_simpleAbility);
    }
  }
}
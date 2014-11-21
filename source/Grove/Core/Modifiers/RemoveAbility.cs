namespace Grove.Modifiers
{
  public class RemoveAbility : Modifier, ICardModifier
  {
    private readonly Static _simpleAbility;
    private SimpleAbilities _simpleAbilties;
    private RemoveFromList<Static> _modifier;

    private RemoveAbility() {}

    public RemoveAbility(Static simpleAbility)
    {
      _simpleAbility = simpleAbility;
    }

    public override void Apply(SimpleAbilities abilities)
    {
      _modifier = new RemoveFromList<Static>(_simpleAbility);
      _modifier.Initialize(ChangeTracker);
      abilities.AddModifier(_modifier);
      _simpleAbilties = abilities;
    }

    protected override void Unapply()
    {
      _simpleAbilties.RemoveModifier(_modifier);
    }
  }
}
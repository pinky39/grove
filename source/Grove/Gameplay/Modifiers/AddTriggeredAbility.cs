namespace Grove.Gameplay.Modifiers
{
  using Abilities;
  using Targeting;

  public class AddTriggeredAbility : Modifier
  {
    private readonly TriggeredAbility _triggeredAbility;
    private TriggeredAbilities _abilties;

    private AddTriggeredAbility() {}

    public AddTriggeredAbility(TriggeredAbility triggeredAbility)
    {
      _triggeredAbility = triggeredAbility;
    }

    public override void Apply(TriggeredAbilities abilities)
    {
      _abilties = abilities;
      _triggeredAbility.Initialize(Target.Card(), Game);
      _abilties.Add(_triggeredAbility);
    }

    protected override void Unapply()
    {
      _abilties.Remove(_triggeredAbility);
    }
  }
}
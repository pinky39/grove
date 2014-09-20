namespace Grove.Modifiers
{
  public class AddTriggeredAbility : Modifier, ICardModifier
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
      _triggeredAbility.Initialize(OwningCard, Game);
      _abilties.Add(_triggeredAbility);
    }

    protected override void Unapply()
    {
      _abilties.Remove(_triggeredAbility);
    }
  }
}
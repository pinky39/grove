namespace Grove.Modifiers
{
  public class AddTriggeredAbility : Modifier, ICardModifier
  {
    private readonly TriggeredAbility _triggeredAbility;
    private TriggeredAbilities _abilities;
    private AddToList<TriggeredAbility> _modifier;

    private AddTriggeredAbility() {}

    public AddTriggeredAbility(TriggeredAbility triggeredAbility)
    {
      _triggeredAbility = triggeredAbility;
    }
    
    public override void Apply(TriggeredAbilities abilities)
    {
      _abilities = abilities;
      _modifier = new AddToList<TriggeredAbility>(_triggeredAbility);
      _modifier.Initialize(ChangeTracker);
      _triggeredAbility.Initialize(OwningCard, Game);      

      _abilities.AddModifier(_modifier);
    }

    protected override void Unapply()
    {
      _abilities.RemoveModifier(_modifier);      
    }
  }
}
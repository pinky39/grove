namespace Grove.Core.Modifiers
{
  public class AddTriggeredAbility : Modifier
  {
    private TriggeredAbility _ability;
    private TriggeredAbilities _abilties;
    
    public ITriggeredAbilityFactory Ability { get; set; }

    public override void Apply(TriggeredAbilities abilities)
    {
      _abilties = abilities;
      _ability = Ability.Create(Target, Source);
      _abilties.Add(_ability);
    }

    protected override void Unapply()
    {
     _abilties.Remove(_ability);
    }
  }
}
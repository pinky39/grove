namespace Grove.Core.Details.Cards.Modifiers
{
  using Targeting;

  public class AddTriggeredAbility : Modifier
  {
    private TriggeredAbility _ability;
    private TriggeredAbilities _abilties;

    public ITriggeredAbilityFactory Ability { get; set; }

    public override void Apply(TriggeredAbilities abilities)
    {
      _abilties = abilities;
      _ability = Ability.Create((Card) Target, Source);
      _abilties.Add(_ability);
    }

    protected override void Unapply()
    {
      _abilties.Remove(_ability);
    }
  }
}
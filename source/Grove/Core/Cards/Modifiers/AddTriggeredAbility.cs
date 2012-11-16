namespace Grove.Core.Cards.Modifiers
{
  using System;
  using Targeting;


  public class AddTriggeredAbility : Modifier
  {
    private TriggeredAbility _ability;
    private TriggeredAbilities _abilties;

    public ITriggeredAbilityFactory Ability { get; set; }

    public override void Apply(TriggeredAbilities abilities)
    {
      _abilties = abilities;
      _ability = Ability.Create((Card) Target, Source, Game);
      _abilties.Add(_ability);
    }

    protected override void Unapply()
    {
      _abilties.Remove(_ability);
    }
  }
}
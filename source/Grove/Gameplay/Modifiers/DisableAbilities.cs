namespace Grove.Core.Modifiers
{
  public class DisableAbilities : Modifier
  {
    private ActivatedAbilities _activatedAbilities;
    private StaticAbilities _staticAbilties;
    private TriggeredAbilities _triggeredAbilities;

    public override void Apply(ActivatedAbilities abilities)
    {
      abilities.DisableAll();
      _activatedAbilities = abilities;
    }

    public override void Apply(StaticAbilities abilities)
    {
      abilities.Disable();
      _staticAbilties = abilities;
    }

    public override void Apply(TriggeredAbilities abilities)
    {
      abilities.DisableAll();
      _triggeredAbilities = abilities;
    }

    protected override void Unapply()
    {
      _activatedAbilities.EnableAll();
      _staticAbilties.Enable();
      _triggeredAbilities.EnableAll();
    }
  }
}
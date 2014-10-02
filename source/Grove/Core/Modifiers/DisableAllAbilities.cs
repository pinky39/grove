namespace Grove.Modifiers
{
  using System;
  using System.Linq;

  public class DisableAllAbilities : Modifier, ICardModifier
  {
    private ActivatedAbilities _activatedAbilities;
    private SimpleAbilities _simpleAbilties;
    private TriggeredAbilities _triggeredAbilities;

    private readonly Func<ActivatedAbility, bool> _activated;
    private readonly Func<SimpleAbility, bool> _simple;
    private readonly Func<TriggeredAbility, bool> _triggered;

    private DisableAllAbilities() { }

    public DisableAllAbilities(bool activated = true, bool simple = true, bool triggered = true)
      : this(a => activated, s => simple, t => triggered) {}

    public DisableAllAbilities(Func<ActivatedAbility, bool> activated, Func<SimpleAbility, bool> simple, Func<TriggeredAbility, bool> triggered)
    {
      _activated = activated;
      _simple = simple;
      _triggered = triggered;
    }

    public override void Apply(ActivatedAbilities abilities)
    {
      _activatedAbilities = new ActivatedAbilities(abilities.GetFiltered(_activated));
      _activatedAbilities.DisableAll();
    }

    public override void Apply(SimpleAbilities abilities)
    {
        _simpleAbilties = new SimpleAbilities(abilities.GetFiltered(_simple).Select(x => x.Value));
        _simpleAbilties.Initialize(OwningCard, Game);
        _simpleAbilties.Disable();
    }

    public override void Apply(TriggeredAbilities abilities)
    {
        _triggeredAbilities = new TriggeredAbilities(abilities.GetFiltered(_triggered));
        _triggeredAbilities.DisableAll();
    }

    protected override void Unapply()
    {
      _activatedAbilities.EnableAll();
      _simpleAbilties.Enable();
      _triggeredAbilities.EnableAll();
    }
  }
}
namespace Grove.Modifiers
{
  using System.Collections.Generic;

  public class DisableAllAbilities : Modifier, ICardModifier
  {
    private readonly bool _activated;
    private readonly bool _simple;
    private readonly bool _triggered;

    private ActivatedAbilities _activatedAbilities;
    private SimpleAbilities _simpleAbilties;
    private TriggeredAbilities _triggeredAbilities;

    private SetList<ActivatedAbility> _activatedAbilitiesModifier;
    private SetList<TriggeredAbility> _triggeredAbilitiesModifier;
    private SetList<Static> _simpleAbilitiesModifier;

    private DisableAllAbilities() {}

    public DisableAllAbilities(bool activated = false, bool simple = false, bool triggered = false)
    {
      _activated = activated;
      _simple = simple;
      _triggered = triggered;
    }

    public override void Apply(ActivatedAbilities abilities)
    {
      if (!_activated)
        return;

      _activatedAbilities = abilities;
      _activatedAbilitiesModifier = new SetList<ActivatedAbility>(
        new List<ActivatedAbility>());
      
      _activatedAbilitiesModifier.Initialize(ChangeTracker);
      _activatedAbilities.AddModifier(_activatedAbilitiesModifier);      
    }

    public override void Apply(SimpleAbilities abilities)
    {
      if (!_simple)
        return;

      _simpleAbilties = abilities;
      _simpleAbilitiesModifier = new SetList<Static>(
        new List<Static>());
      _simpleAbilitiesModifier.Initialize(ChangeTracker);
      _simpleAbilties.AddModifier(_simpleAbilitiesModifier);
    }

    public override void Apply(TriggeredAbilities abilities)
    {
      if (!_triggered)
        return;

      _triggeredAbilities = abilities;
      _triggeredAbilitiesModifier = new SetList<TriggeredAbility>(
        new List<TriggeredAbility>());
      _triggeredAbilitiesModifier.Initialize(ChangeTracker);
      _triggeredAbilities.AddModifier(_triggeredAbilitiesModifier);
    }

    protected override void Unapply()
    {
      if (_activated)
      {
        _activatedAbilities.RemoveModifier(_activatedAbilitiesModifier);
      }

      if (_simple)
      {
        _simpleAbilties.RemoveModifier(_simpleAbilitiesModifier);
      }

      if (_triggered)
      {
        _triggeredAbilities.RemoveModifier(_triggeredAbilitiesModifier);
      }
    }
  }
}
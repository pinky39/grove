namespace Grove.Modifiers
{
    public class DisableAllAbilities : Modifier, ICardModifier
    {
        private ActivatedAbilities _activatedAbilities;
        private SimpleAbilities _simpleAbilties;
        private TriggeredAbilities _triggeredAbilities;

        private readonly bool _activated;
        private readonly bool _simple;
        private readonly bool _triggered;

        private DisableAllAbilities() { }

        public DisableAllAbilities(bool activated = true, bool simple = true, bool triggered = true)
        {
            _activated = activated;
            _simple = simple;
            _triggered = triggered;
        }

        public override void Apply(ActivatedAbilities abilities)
        {
            if (_activated)
            {
                abilities.DisableAll();
                _activatedAbilities = abilities;
            }
        }

        public override void Apply(SimpleAbilities abilities)
        {
            if (_simple)
            {
                abilities.Disable();
                _simpleAbilties = abilities;
            }
        }

        public override void Apply(TriggeredAbilities abilities)
        {
            if (_triggered)
            {
                abilities.DisableAll();
                _triggeredAbilities = abilities;
            }
        }

        protected override void Unapply()
        {
            if (_activated)
            {
                _activatedAbilities.EnableAll();
            }
            if (_simple)
            {
                _simpleAbilties.Enable();
            }
            if (_triggered)
            {
                _triggeredAbilities.EnableAll();
            }
        }
    }
}
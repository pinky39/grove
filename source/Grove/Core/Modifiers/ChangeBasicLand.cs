namespace Grove.Modifiers
{
    using Grove.Costs;

    public class ChangeBasicLand : Modifier, ICardModifier
    {
        private readonly string _changeTo;
        private ActivatedAbilities _abilities;
        private ActivatedAbility _addedAbility;
        private CardTypeCharacteristic _cardType;
        private bool _isBasicLand;
        private ActivatedAbility _removedAbility;
        private CardTypeSetter _typeSetter;
        private bool _replace;

        private ChangeBasicLand() { }

        public ChangeBasicLand(string changeTo, bool replace = true)
        {
            _changeTo = changeTo;
            _replace = replace;
        }

        public override void Apply(ActivatedAbilities abilities)
        {
            _abilities = abilities;
            if (_isBasicLand)
            {
                _removedAbility = _abilities.RemoveFirst();
            }

            var basicLandMana = Mana.GetBasicLandMana(_changeTo);

            var ap = new ManaAbilityParameters
            {
                Text = "{{T}}: Add {0} to your mana pool.",
            };

            ap.ManaAmount(basicLandMana);
            _addedAbility = new ManaAbility(ap);
            _addedAbility.Initialize(OwningCard, Game);
            _abilities.Add(_addedAbility);
        }

        public override void Apply(CardTypeCharacteristic cardType)
        {
            _cardType = cardType;
            _isBasicLand = cardType.Value.BasicLand;

            var type = _replace
                ? _cardType.Value.ReplaceLandTypeWith(_changeTo)
                : _cardType.Value.AddBasicLandType(_changeTo);

            _typeSetter = new CardTypeSetter(type);
            _typeSetter.Initialize(ChangeTracker);

            _cardType.AddModifier(_typeSetter);
        }

        protected override void Unapply()
        {
            _cardType.RemoveModifier(_typeSetter);
            _abilities.Remove(_addedAbility);

            if (_removedAbility != null)
                _abilities.Add(_removedAbility);
        }
    }
}
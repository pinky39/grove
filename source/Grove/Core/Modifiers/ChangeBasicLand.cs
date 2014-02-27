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

    private ChangeBasicLand() {}

    public ChangeBasicLand(string changeTo)
    {
      _changeTo = changeTo;
    }

    public override void Apply(ActivatedAbilities abilities)
    {
      _abilities = abilities;
      if (_isBasicLand)
      {
        _removedAbility = _abilities.RemoveFirst();
      }

      var basicLandMana = Mana.GetBasicLandMana(_changeTo);

      var ap = new ManaAbility.Parameters
        {
          Text = "{{T}}: Add {0} to your mana pool.",
          Cost = new Tap(),
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

      var type = _cardType.Value.ReplaceBasicLandTypeWith(_changeTo);

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
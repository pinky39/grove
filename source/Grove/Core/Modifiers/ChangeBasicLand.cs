namespace Grove.Core.Modifiers
{
  using Grove.Core.Mana;
  using Grove.Core.Targeting;

  public class ChangeBasicLand : Modifier
  {
    public string ChangeTo;
    private ActivatedAbilities _abilities;
    private ActivatedAbility _addedAbility;
    private CardTypeCharacteristic _cardType;
    private bool _isBasicLand;
    private ActivatedAbility _removedAbility;
    private CardTypeSetter _typeSetter;

    public override void Apply(ActivatedAbilities abilities)
    {
      _abilities = abilities;
      if (_isBasicLand)
      {
        _removedAbility = _abilities.RemoveFirst();
      }

      var manaUnit = ManaUnit.GetBasicLandMana(ChangeTo.ToLowerInvariant());

      _addedAbility = Builder
        .ManaAbility(
          manaUnit,
          string.Format("{{T}}: Add {0} to your mana pool.", manaUnit))
        .Create(Target.Card(), Game);

      _abilities.Add(_addedAbility);
    }

    public override void Apply(CardTypeCharacteristic cardType)
    {
      _cardType = cardType;
      _isBasicLand = cardType.Value.BasicLand;

      var type = _cardType.Value.ReplaceBasicLandTypeWith(ChangeTo);
      _typeSetter = new CardTypeSetter(type, ChangeTracker);
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
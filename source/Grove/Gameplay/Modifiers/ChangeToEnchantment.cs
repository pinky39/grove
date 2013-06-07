namespace Grove.Gameplay.Modifiers
{
  using Characteristics;

  public class ChangeToEnchantment : Modifier
  {
    private Power _cardPower;
    private Toughness _cardToughness;
    private CardTypeCharacteristic _cardType;
    private IntegerSetter _integerSetter = new IntegerSetter();
    private CardTypeSetter _typeSetter;

    public override void Apply(CardTypeCharacteristic cardType)
    {
      _cardType = cardType;
      _typeSetter = new CardTypeSetter("Enchantment");
      _typeSetter.Initialize(ChangeTracker);
      _cardType.AddModifier(_typeSetter);
    }

    protected override void Initialize()
    {
      _integerSetter.Initialize(ChangeTracker);
    }

    public override void Apply(Power power)
    {
      _cardPower = power;      
      _integerSetter.Initialize(ChangeTracker);
      _cardPower.AddModifier(_integerSetter);
    }

    public override void Apply(Toughness toughness)
    {
      _cardToughness = toughness;
      _cardToughness.AddModifier(_integerSetter);
    }

    protected override void Unapply()
    {
      _cardType.RemoveModifier(_typeSetter);
      _cardPower.RemoveModifier(_integerSetter);
      _cardToughness.RemoveModifier(_integerSetter);
    }
  }
}
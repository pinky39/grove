namespace Grove.Gameplay.Modifiers
{
  using Card.Characteristics;

  public class ChangeToEnchantment : Modifier
  {
    private CardTypeCharacteristic _cardType;
    private CardTypeSetter _typeSetter;
    private Power _cardPower;
    private StrenghtSetter _strenghtSetter = new StrenghtSetter(null);
    private Toughness _cardToughness;

    public override void Apply(CardTypeCharacteristic cardType)
    {
      _cardType = cardType;
      _typeSetter = new CardTypeSetter("Enchantment");
      _typeSetter.Initialize(ChangeTracker);
      _cardType.AddModifier(_typeSetter);
    }

    protected override void Initialize()
    {
      _strenghtSetter.Initialize(ChangeTracker);
    }

    public override void Apply(Power power)
    {
      _cardPower = power;
      _strenghtSetter = new StrenghtSetter(null);
      _strenghtSetter.Initialize(ChangeTracker);
      _cardPower.AddModifier(_strenghtSetter);
    }

    public override void Apply(Toughness toughness)
    {
      _cardToughness = toughness;      
      _cardToughness.AddModifier(_strenghtSetter);
    }
    
    protected override void Unapply()
    {
      _cardType.RemoveModifier(_typeSetter);
      _cardPower.RemoveModifier(_strenghtSetter);
      _cardToughness.RemoveModifier(_strenghtSetter);
    }
  }
}
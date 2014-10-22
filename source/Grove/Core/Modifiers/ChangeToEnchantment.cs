namespace Grove.Modifiers
{
  public class ChangeToEnchantment : Modifier, ICardModifier
  {
    private Strenght _strenght;
    private CardTypeCharacteristic _cardType;
    private readonly IntegerSetter _integerSetter = new IntegerSetter();
    private CardTypeSetter _typeSetter;

    public override void Apply(CardTypeCharacteristic cardType)
    {
      _cardType = cardType;
      _typeSetter = new CardTypeSetter("enchantment");
      _typeSetter.Initialize(ChangeTracker);
      _cardType.AddModifier(_typeSetter);
    }

    protected override void Initialize()
    {
      _integerSetter.Initialize(ChangeTracker);
    }

    public override void Apply(Strenght strenght)
    {
      _strenght = strenght;
      _strenght.AddPowerModifier(_integerSetter);
      _strenght.AddToughnessModifier(_integerSetter);
    }        

    protected override void Unapply()
    {
      _cardType.RemoveModifier(_typeSetter);
      _strenght.RemovePowerModifier(_integerSetter);
      _strenght.RemoveToughnessModifier(_integerSetter);      
    }
  }
}
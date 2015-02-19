namespace Grove.Modifiers
{
  public class ChangeToEnchantment : Modifier, ICardModifier
  {
    private Strenght _strenght;
    private TypeOfCard _typeOfCard;
    private readonly IntegerSetter _integerSetter = new IntegerSetter();
    private CardTypeSetter _typeSetter;

    public override void Apply(TypeOfCard typeOfCard)
    {
      _typeOfCard = typeOfCard;
      _typeSetter = new CardTypeSetter("enchantment");      
      _typeSetter.Initialize(ChangeTracker);
      _typeOfCard.AddModifier(_typeSetter);
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
      _typeOfCard.RemoveModifier(_typeSetter);
      _strenght.RemovePowerModifier(_integerSetter);
      _strenght.RemoveToughnessModifier(_integerSetter);      
    }
  }
}
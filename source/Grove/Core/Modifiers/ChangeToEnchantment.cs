namespace Grove.Modifiers
{
  public class ChangeToEnchantment : Modifier, ICardModifier
  {
    private Strength _strength;
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

    public override void Apply(Strength strength)
    {
      _strength = strength;
      _strength.AddPowerModifier(_integerSetter);
      _strength.AddToughnessModifier(_integerSetter);
    }

    protected override void Unapply()
    {
      _typeOfCard.RemoveModifier(_typeSetter);
      _strength.RemovePowerModifier(_integerSetter);
      _strength.RemoveToughnessModifier(_integerSetter);
    }
  }
}
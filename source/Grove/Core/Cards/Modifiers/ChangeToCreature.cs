namespace Grove.Core.Cards.Modifiers
{
  using Grove.Core.Messages;
  using Grove.Core.Targeting;
  using Mana;

  public class ChangeToCreature : Modifier
  {
    private CardColors _cardColors;
    private Power _cardPower;
    private Toughness _cardToughness;
    private CardTypeCharacteristic _cardType;
    private ColorsSetter _colorsSetter;
    private StrenghtSetter _powerSetter;
    private StrenghtSetter _toughnessSetter;
    private CardTypeSetter _typeSetter;

    public ManaColors Colors { get; set; }
    public Value Power { get; set; }
    public Value Toughness { get; set; }
    public string Type { get; set; }

    public override void Apply(CardColors colors)
    {
      _cardColors = colors;
      _colorsSetter = new ColorsSetter(Colors, ChangeTracker);
      _cardColors.AddModifier(_colorsSetter);
    }

    public override void Apply(Power power)
    {
      _cardPower = power;
      _powerSetter = new StrenghtSetter(Power.GetValue(X), ChangeTracker);
      _cardPower.AddModifier(_powerSetter);
    }

    public override void Apply(Toughness toughness)
    {
      _cardToughness = toughness;
      _toughnessSetter = new StrenghtSetter(Toughness.GetValue(X), ChangeTracker);
      _cardToughness.AddModifier(_toughnessSetter);
    }

    public override void Apply(CardTypeCharacteristic cardType)
    {
      _cardType = cardType;
      _typeSetter = new CardTypeSetter(Type, ChangeTracker);
      _cardType.AddModifier(_typeSetter);

      Game.Publish(new CardChangedType {Card = Target.Card()});
    }

    protected override void Unapply()
    {
      _cardColors.RemoveModifier(_colorsSetter);
      _cardPower.RemoveModifier(_powerSetter);
      _cardToughness.RemoveModifier(_toughnessSetter);
      _cardType.RemoveModifier(_typeSetter);
    }
  }
}
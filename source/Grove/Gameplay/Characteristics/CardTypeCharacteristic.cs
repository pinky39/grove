namespace Grove.Gameplay.Characteristics
{
  using Infrastructure;
  using Messages;
  using Modifiers;

  public class CardTypeCharacteristic : Characteristic<CardType>, IAcceptsCardModifier
  {
    private Card _card;
    private CardTypeCharacteristic() {}
    public CardTypeCharacteristic(CardType value) : base(value) {}

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public override void Initialize(Game game, IHashDependancy hashDependancy)
    {
      base.Initialize(game, hashDependancy);
      _card = (Card) hashDependancy;
    }

    protected override void OnCharacteristicChanged(CardType oldValue, CardType newValue)
    {
      Publish(new TypeChanged
        {
          Card = _card,
          OldValue = oldValue,
          NewValue = newValue
        });
    }
  }
}